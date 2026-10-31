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
