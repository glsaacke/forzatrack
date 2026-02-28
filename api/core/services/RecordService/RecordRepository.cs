using MySql.Data.MySqlClient;
using api.core.models;

namespace api.core.services.RecordService
{
    public class RecordRepository : IRecordRepository
    {
        private readonly string _connectionString;

        public RecordRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Record>> GetAllRecordsAsync()
        {
            var records = new List<Record>();

            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "SELECT record_id, user_id, car_id, event, class_rank, time_min, time_sec, time_ms, cpu_diff, deleted FROM Records";
            using var cmd = new MySqlCommand(sql, con);
            using var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                records.Add(MapRecord(reader, includeDate: false));
            }

            return records;
        }

        public async Task<Record?> GetRecordByIdAsync(int id)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "SELECT record_id, user_id, car_id, event, class_rank, time_min, time_sec, time_ms, cpu_diff, deleted FROM Records WHERE record_id = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapRecord(reader, includeDate: false) : null;
        }

        public async Task CreateRecordAsync(Record record)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = @"INSERT INTO Records(user_id, car_id, event, class_rank, time_min, time_sec, time_ms, cpu_diff, deleted, date) 
                                 VALUES(@user_id, @car_id, @event, @class_rank, @time_min, @time_sec, @time_ms, @cpu_diff, 0, @date)";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@user_id", record.UserId);
            cmd.Parameters.AddWithValue("@car_id", record.CarId);
            cmd.Parameters.AddWithValue("@event", record.Event);
            cmd.Parameters.AddWithValue("@class_rank", record.ClassRank);
            cmd.Parameters.AddWithValue("@time_min", record.TimeMin);
            cmd.Parameters.AddWithValue("@time_sec", record.TimeSec);
            cmd.Parameters.AddWithValue("@time_ms", record.TimeMs);
            cmd.Parameters.AddWithValue("@cpu_diff", record.CpuDiff);
            cmd.Parameters.AddWithValue("@date", record.AddDate);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> UpdateRecordAsync(Record record, int id)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = @"UPDATE Records SET user_id = @user_id, car_id = @car_id, event = @event, class_rank = @class_rank, 
                                 time_min = @time_min, time_sec = @time_sec, time_ms = @time_ms, cpu_diff = @cpu_diff WHERE record_id = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@user_id", record.UserId);
            cmd.Parameters.AddWithValue("@car_id", record.CarId);
            cmd.Parameters.AddWithValue("@event", record.Event);
            cmd.Parameters.AddWithValue("@class_rank", record.ClassRank);
            cmd.Parameters.AddWithValue("@time_min", record.TimeMin);
            cmd.Parameters.AddWithValue("@time_sec", record.TimeSec);
            cmd.Parameters.AddWithValue("@time_ms", record.TimeMs);
            cmd.Parameters.AddWithValue("@cpu_diff", record.CpuDiff);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task DeleteRecordAsync(int id)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "DELETE FROM Records WHERE record_id = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> SetRecordDeletedAsync(int id)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "UPDATE Records SET deleted = 1 WHERE record_id = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<List<Record>> GetRecordsByUserIdAsync(int id)
        {
            var records = new List<Record>();

            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "SELECT record_id, user_id, car_id, event, class_rank, time_min, time_sec, time_ms, cpu_diff, deleted, date FROM Records WHERE user_id = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                records.Add(MapRecord(reader, includeDate: true));
            }

            return records;
        }

        private static Record MapRecord(MySqlDataReader reader, bool includeDate) => new()
        {
            RecordId = reader.GetInt32("record_id"),
            UserId = reader.GetInt32("user_id"),
            CarId = reader.GetInt32("car_id"),
            Event = reader.GetString("event"),
            ClassRank = reader.GetString("class_rank"),
            TimeMin = reader.GetInt32("time_min"),
            TimeSec = reader.GetInt32("time_sec"),
            TimeMs = reader.GetInt32("time_ms"),
            CpuDiff = reader.GetString("cpu_diff"),
            AddDate = includeDate ? reader.GetDateTime("date") : default,
            Deleted = reader.GetInt32("deleted"),
        };
    }
}