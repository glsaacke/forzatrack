using MySql.Data.MySqlClient;
using api.core.models;

namespace api.core.services.BuildService
{
    public class BuildRepository : IBuildRepository
    {
        private readonly string _connectionString;

        public BuildRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Build>> GetAllBuildsAsync()
        {
            var builds = new List<Build>();

            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "SELECT build_id, user_id, car_id, `rank`, speed_st, handling_st, acceleration_st, launch_st, braking_st, offroad_st, top_speed, zero_to_sixty, deleted FROM Builds";
            using var cmd = new MySqlCommand(sql, con);
            using var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                builds.Add(MapBuild(reader));
            }

            return builds;
        }

        public async Task<Build?> GetBuildByIdAsync(int id)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "SELECT build_id, user_id, car_id, `rank`, speed_st, handling_st, acceleration_st, launch_st, braking_st, offroad_st, top_speed, zero_to_sixty, deleted FROM Builds WHERE build_id = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapBuild(reader) : null;
        }

        public async Task CreateBuildAsync(Build build)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = @"INSERT INTO Builds(user_id, car_id, `rank`, speed_st, handling_st, acceleration_st, launch_st, braking_st, offroad_st, top_speed, zero_to_sixty, deleted) 
                                 VALUES(@user_id, @car_id, @rank, @speed_st, @handling_st, @acceleration_st, @launch_st, @braking_st, @offroad_st, @top_speed, @zero_to_sixty, 0)";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@user_id", build.UserId);
            cmd.Parameters.AddWithValue("@car_id", build.CarId);
            cmd.Parameters.AddWithValue("@rank", build.Rank);
            cmd.Parameters.AddWithValue("@speed_st", build.SpeedST);
            cmd.Parameters.AddWithValue("@handling_st", build.HandlingST);
            cmd.Parameters.AddWithValue("@acceleration_st", build.AccelerationST);
            cmd.Parameters.AddWithValue("@launch_st", build.LaunchST);
            cmd.Parameters.AddWithValue("@braking_st", build.BrakingST);
            cmd.Parameters.AddWithValue("@offroad_st", build.OffroadST);
            cmd.Parameters.AddWithValue("@top_speed", build.TopSpeed);
            cmd.Parameters.AddWithValue("@zero_to_sixty", build.ZeroToSixty);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> UpdateBuildAsync(Build build, int id)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = @"UPDATE Builds SET user_id = @user_id, car_id = @car_id, `rank` = @rank, speed_st = @speed_st, 
                                 handling_st = @handling_st, acceleration_st = @acceleration_st, launch_st = @launch_st, 
                                 braking_st = @braking_st, offroad_st = @offroad_st, top_speed = @top_speed, 
                                 zero_to_sixty = @zero_to_sixty WHERE build_id = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@user_id", build.UserId);
            cmd.Parameters.AddWithValue("@car_id", build.CarId);
            cmd.Parameters.AddWithValue("@rank", build.Rank);
            cmd.Parameters.AddWithValue("@speed_st", build.SpeedST);
            cmd.Parameters.AddWithValue("@handling_st", build.HandlingST);
            cmd.Parameters.AddWithValue("@acceleration_st", build.AccelerationST);
            cmd.Parameters.AddWithValue("@launch_st", build.LaunchST);
            cmd.Parameters.AddWithValue("@braking_st", build.BrakingST);
            cmd.Parameters.AddWithValue("@offroad_st", build.OffroadST);
            cmd.Parameters.AddWithValue("@top_speed", build.TopSpeed);
            cmd.Parameters.AddWithValue("@zero_to_sixty", build.ZeroToSixty);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task DeleteBuildAsync(int id)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "DELETE FROM Builds WHERE build_id = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> SetBuildDeletedAsync(int id)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "UPDATE Builds SET deleted = 1 WHERE build_id = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        private static Build MapBuild(MySqlDataReader reader) => new()
        {
            BuildId = reader.GetInt32("build_id"),
            UserId = reader.GetInt32("user_id"),
            CarId = reader.GetInt32("car_id"),
            Rank = reader.GetInt32("rank"),
            SpeedST = reader.GetDouble("speed_st"),
            HandlingST = reader.GetDouble("handling_st"),
            AccelerationST = reader.GetDouble("acceleration_st"),
            LaunchST = reader.GetDouble("launch_st"),
            BrakingST = reader.GetDouble("braking_st"),
            OffroadST = reader.GetDouble("offroad_st"),
            TopSpeed = reader.GetDouble("top_speed"),
            ZeroToSixty = reader.GetDouble("zero_to_sixty"),
            Deleted = reader.GetInt32("deleted"),
        };
    }
}