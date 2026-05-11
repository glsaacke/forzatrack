// One-time migration script: hash all plain-text passwords in the Users table.
// Run this BEFORE deploying the updated API binary.
//
// Usage (from the api/ directory):
//   dotnet script MigratePasswords.cs
//
// Or temporarily call MigratePasswords.Run(connectionString) from Program.cs
// then remove it before the next deploy.

using MySqlConnector;

public static class MigratePasswords
{
    public static async Task Run(string connectionString)
    {
        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        // Read all users that appear to have plain-text passwords (BCrypt hashes start with '$2')
        var selectCmd = new MySqlCommand(
            "SELECT user_id, password FROM Users WHERE deleted = 0 AND password NOT LIKE '$2%'",
            connection);

        var rows = new List<(int Id, string PlainPassword)>();
        await using (var reader = await selectCmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                rows.Add((reader.GetInt32(0), reader.GetString(1)));
            }
        }

        Console.WriteLine($"Found {rows.Count} user(s) with plain-text passwords. Hashing...");

        foreach (var (id, plainPassword) in rows)
        {
            string hashed = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            var updateCmd = new MySqlCommand(
                "UPDATE Users SET password = @hash WHERE user_id = @id",
                connection);
            updateCmd.Parameters.AddWithValue("@hash", hashed);
            updateCmd.Parameters.AddWithValue("@id", id);
            await updateCmd.ExecuteNonQueryAsync();

            Console.WriteLine($"  Hashed password for user_id={id}");
        }

        Console.WriteLine("Migration complete.");
    }
}
