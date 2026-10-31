# Migrate Existing Events to Courses Implementation Plan

A one-time migration script that seeds the six FH5 courses into the new `Courses` table and updates every existing `Record` row to reference the correct `course_id` by matching on the old `event` string. This must run after the `Courses` table exists (Feature 1) and before the `event` column is dropped (Feature 3).

---

## Tasks

### 1. Create the migration class
**File:** `api/MigrateCourses.cs`

Follow the same pattern as the existing `MigratePasswords.cs` — a static class with a `Run(string connectionString)` method using raw `MySqlConnector` (not EF Core), so it can be executed independently of the application startup.

```csharp
// One-time migration: seed FH5 courses and link existing records to them.
// Call MigrateCourses.Run(connectionString) from Program.cs once, then remove the call.

using MySqlConnector;

public static class MigrateCourses
{
    private static readonly (string Name, string Game)[] Fh5Courses =
    [
        ("Goliath",       "FH5"),
        ("Colossus",      "FH5"),
        ("Gauntlet",      "FH5"),
        ("Titan",         "FH5"),
        ("Marathon",      "FH5"),
        ("Vulcan Sprint", "FH5"),
    ];

    public static async Task Run(string connectionString)
    {
        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        // --- Step 1: Insert FH5 courses (skip if already present) ---
        var courseIds = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var (name, game) in Fh5Courses)
        {
            // Check if this course already exists to make the script idempotent
            var checkCmd = new MySqlCommand(
                "SELECT course_id FROM Courses WHERE name = @name AND game = @game LIMIT 1",
                connection);
            checkCmd.Parameters.AddWithValue("@name", name);
            checkCmd.Parameters.AddWithValue("@game", game);

            var existingId = await checkCmd.ExecuteScalarAsync();
            if (existingId != null)
            {
                courseIds[name] = Convert.ToInt32(existingId);
                Console.WriteLine($"  Course already exists: {name} ({game}), id={courseIds[name]}");
                continue;
            }

            var insertCmd = new MySqlCommand(
                "INSERT INTO Courses (name, game, deleted) VALUES (@name, @game, 0)",
                connection);
            insertCmd.Parameters.AddWithValue("@name", name);
            insertCmd.Parameters.AddWithValue("@game", game);
            await insertCmd.ExecuteNonQueryAsync();

            var idCmd = new MySqlCommand("SELECT LAST_INSERT_ID()", connection);
            courseIds[name] = Convert.ToInt32(await idCmd.ExecuteScalarAsync());
            Console.WriteLine($"  Inserted course: {name} ({game}), id={courseIds[name]}");
        }

        // --- Step 2: Update Records to set course_id from the old event string ---
        int updated = 0;
        int skipped = 0;

        var recordsCmd = new MySqlCommand(
            "SELECT record_id, event FROM Records WHERE deleted = 0",
            connection);

        var rows = new List<(int Id, string Event)>();
        await using (var reader = await recordsCmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
                rows.Add((reader.GetInt32(0), reader.GetString(1)));
        }

        Console.WriteLine($"Found {rows.Count} record(s) to migrate.");

        foreach (var (recordId, eventName) in rows)
        {
            if (!courseIds.TryGetValue(eventName, out int courseId))
            {
                Console.WriteLine($"  WARNING: No course found for event '{eventName}' (record_id={recordId}). Skipping.");
                skipped++;
                continue;
            }

            var updateCmd = new MySqlCommand(
                "UPDATE Records SET course_id = @courseId WHERE record_id = @recordId",
                connection);
            updateCmd.Parameters.AddWithValue("@courseId", courseId);
            updateCmd.Parameters.AddWithValue("@recordId", recordId);
            await updateCmd.ExecuteNonQueryAsync();
            updated++;
        }

        Console.WriteLine($"Migration complete. Updated: {updated}, Skipped: {skipped}.");
    }
}
```

### 2. Add the nullable `course_id` column to `Records`

Before running the migration script, add a nullable FK column to the `Records` table. Making it nullable first allows existing rows to be updated safely before making it required in Feature 3.

```sql
ALTER TABLE Records
    ADD COLUMN course_id INT NULL,
    ADD CONSTRAINT fk_records_course
        FOREIGN KEY (course_id) REFERENCES Courses(course_id);
```

### 3. Wire the migration into `Program.cs` temporarily
**File:** `api/Program.cs`

Add a one-time call near the top of the file, after `Env.Load()` and `connectionString` is populated:

```csharp
// TEMPORARY — remove after running once
await MigrateCourses.Run(connectionString);
```

> **Important:** Remove this line before the next deploy. The script is idempotent (it skips already-inserted courses) but there is no benefit to running it on every startup.

### 4. Verify the migration

After deploying and running, confirm in the database:
```sql
SELECT * FROM Courses;
SELECT record_id, event, course_id FROM Records LIMIT 20;
```

All records should have a non-null `course_id` matching their old `event` string. Any rows with `NULL` in `course_id` indicate an event name that didn't match — investigate and fix manually.

---

## Notes
- The script is **idempotent**: re-running it will skip courses that already exist and update only records that already have a matching course.
- Records with an unrecognised `event` value will be skipped with a warning printed to stdout. Review these manually before Feature 3 makes `course_id` non-nullable.
- Do **not** drop the old `event` column yet — that happens in Feature 3.
- The `course_id` column should be added to the DB before the script runs (Task 2 above), but the `AppDbContext` mapping for `Record` should not be updated until Feature 3, since the column is not yet authoritative.
