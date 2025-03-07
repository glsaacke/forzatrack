using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using api.core.models;
using api.core.data;

namespace api.core.services.RecordService
{
    public class RecordRepository : IRecordRepository
    {
        private readonly string cs;
        public RecordRepository(string cs){
            this.cs = cs;
        }

        public List<Record> GetAllRecords(){
            List<Record> records = new List<Record>();

            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "SELECT record_id, user_id, car_id, event, class_rank, time_min, time_sec, time_ms, cpu_diff, deleted FROM Records";

            using var cmd = new MySqlCommand(stm, con);

            using var reader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            foreach (DataRow row in dataTable.Rows)
            {
                Record record = new Record
                {
                    RecordId = Convert.ToInt32(row["record_id"]),
                    UserId = Convert.ToInt32(row["user_id"]),
                    CarId = Convert.ToInt32(row["car_id"]),
                    Event = row["event"].ToString(),
                    ClassRank = row["class_rank"].ToString(),
                    TimeMin = Convert.ToInt32(row["time_min"]),
                    TimeSec = Convert.ToInt32(row["time_sec"]),
                    TimeMs = Convert.ToInt32(row["time_ms"]),
                    CpuDiff = row["cpu_diff"].ToString(),
                    Deleted = Convert.ToInt32(row["deleted"]),
                };
                records.Add(record);
            }
            return records;
         }
        public Record GetRecordByID(int id){
            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "SELECT record_id, user_id, car_id, event, class_rank, time_min, time_sec, time_ms, cpu_diff, deleted FROM Records WHERE record_id = @id";

            using var cmd = new MySqlCommand(stm, con);

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new Record
                {
                    RecordId = Convert.ToInt32(row["record_id"]),
                    UserId = Convert.ToInt32(row["user_id"]),
                    CarId = Convert.ToInt32(row["car_id"]),
                    Event = row["event"].ToString(),
                    ClassRank = row["class_rank"].ToString(),
                    TimeMin = Convert.ToInt32(row["time_min"]),
                    TimeSec = Convert.ToInt32(row["time_sec"]),
                    TimeMs = Convert.ToInt32(row["time_ms"]),
                    CpuDiff = row["cpu_diff"].ToString(),
                    Deleted = Convert.ToInt32(row["deleted"]),
                };
            }

            return null;
         } 

         public void CreateRecord(Record record){
            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = @"INSERT INTO Records(record_id, user_id, car_id, event, class_rank, time_min, time_sec, time_ms, cpu_diff, deleted, date) VALUES(@record_id, @user_id, @car_id, @event, @class_rank, @time_min, @time_sec, @time_ms, @cpu_diff, @deleted, @date)";

            using var cmd = new MySqlCommand(stm,con);

            cmd.Parameters.AddWithValue("@record_id", record.RecordId);
            cmd.Parameters.AddWithValue("@user_id", record.UserId);
            cmd.Parameters.AddWithValue("@car_id", record.CarId);
            cmd.Parameters.AddWithValue("@event", record.Event);
            cmd.Parameters.AddWithValue("@class_rank", record.ClassRank);
            cmd.Parameters.AddWithValue("@time_min", record.TimeMin);
            cmd.Parameters.AddWithValue("@time_sec", record.TimeSec);
            cmd.Parameters.AddWithValue("@time_ms", record.TimeMs);
            cmd.Parameters.AddWithValue("@cpu_diff", record.CpuDiff);
            cmd.Parameters.AddWithValue("@deleted", record.Deleted);
            cmd.Parameters.AddWithValue("@date", record.AddDate);

            cmd.Prepare();

            cmd.ExecuteNonQuery();
         }

         public bool UpdateRecord(Record record, int id){
            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = $"UPDATE Records SET user_id = @user_id, car_id = @car_id, event = @event, class_rank = @class_rank, time_min = @time_min, time_sec = @time_sec, time_ms = @time_ms, cpu_diff = @cpu_diff WHERE record_id = @id";

            using var cmd = new MySqlCommand(stm, con);
            cmd.Parameters.AddWithValue("@user_id", record.UserId);
            cmd.Parameters.AddWithValue("@car_id", record.CarId);
            cmd.Parameters.AddWithValue("@event", record.Event);
            cmd.Parameters.AddWithValue("@class_rank", record.ClassRank);
            cmd.Parameters.AddWithValue("@time_min", record.TimeMin);
            cmd.Parameters.AddWithValue("@time_sec", record.TimeSec);
            cmd.Parameters.AddWithValue("@time_ms", record.TimeMs);
            cmd.Parameters.AddWithValue("@cpu_diff", record.CpuDiff);
            cmd.Parameters.AddWithValue("@id", id);

            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
         }

         public void DeleteRecord(int id){
            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "DELETE FROM Records WHERE record_id = @id";

            using var cmd = new MySqlCommand(stm, con);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
         }

         public bool SetRecordDeleted(int id){
            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "UPDATE Records SET deleted = 1 WHERE record_id = @id";

            using var cmd = new MySqlCommand(stm, con);
            cmd.Parameters.AddWithValue("@id", id);

            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
         }

        public List<Record> GetRecordsByUserId(int id)
        {
            List<Record> records = new List<Record>();

            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "SELECT record_id, user_id, car_id, event, class_rank, time_min, time_sec, time_ms, cpu_diff, deleted, date FROM Records WHERE user_id = @id";

            using var cmd = new MySqlCommand(stm, con);

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            foreach (DataRow row in dataTable.Rows)
            {
                Record record = new Record
                {
                    RecordId = Convert.ToInt32(row["record_id"]),
                    UserId = Convert.ToInt32(row["user_id"]),
                    CarId = Convert.ToInt32(row["car_id"]),
                    Event = row["event"].ToString(),
                    ClassRank = row["class_rank"].ToString(),
                    TimeMin = Convert.ToInt32(row["time_min"]),
                    TimeSec = Convert.ToInt32(row["time_sec"]),
                    TimeMs = Convert.ToInt32(row["time_ms"]),
                    CpuDiff = row["cpu_diff"].ToString(),
                    AddDate = Convert.ToDateTime(row["date"]),
                    Deleted = Convert.ToInt32(row["deleted"]),
                    
                };
                records.Add(record);
            }
            return records;
        }
    }
}