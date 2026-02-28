using MySql.Data.MySqlClient;
using api.core.models;

namespace api.core.services.UserService
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = new List<User>();

            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "SELECT user_id, username, email, password, deleted FROM Users";
            using var cmd = new MySqlCommand(sql, con);
            using var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(MapUser(reader));
            }

            return users;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "SELECT user_id, username, email, password, deleted FROM Users WHERE user_id = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapUser(reader) : null;
        }

        public async Task CreateUserAsync(CreateUser user)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "INSERT INTO Users(username, email, password, deleted) VALUES(@username, @email, @password, 0)";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.Password);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> UpdateUserAsync(User user, int id)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "UPDATE Users SET username = @username, email = @email, password = @password WHERE user_id = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task DeleteUserAsync(int id)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "DELETE FROM Users WHERE user_id = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> SetUserDeletedAsync(int id)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "UPDATE Users SET deleted = 1 WHERE user_id = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "SELECT user_id, username, email, password, deleted FROM Users WHERE email = @email";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@email", email);

            using var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapUser(reader) : null;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "SELECT user_id, username, email, password, deleted FROM Users WHERE username = @username";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@username", username);

            using var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapUser(reader) : null;
        }

        private static User MapUser(MySqlDataReader reader) => new()
        {
            UserId = reader.GetInt32("user_id"),
            Username = reader.GetString("username"),
            Email = reader.GetString("email"),
            Password = reader.GetString("password"),
            Deleted = reader.GetInt32("deleted"),
        };
    }
}