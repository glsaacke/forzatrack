using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using api.core.models;
using api.core.data;

namespace api.core.services.BuildService
{
    public class BuildRepository : IBuildRepository
    {
        private readonly string cs;
        public BuildRepository(string cs){
            this.cs = cs;
        }

         public List<Build> GetAllBuilds(){
            List<Build> builds = new List<Build>();

            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "SELECT build_id, user_id, car_id, `rank`, speed_st, handling_st, acceleration_st, launch_st, braking_st, offroad_st, top_speed, zero_to_sixty, deleted FROM Builds";

            using var cmd = new MySqlCommand(stm, con);

            using var reader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            foreach (DataRow row in dataTable.Rows)
            {
                Build build = new Build
                {
                    BuildId = Convert.ToInt32(row["build_id"]),
                    UserId = Convert.ToInt32(row["user_id"]),
                    CarId = Convert.ToInt32(row["car_id"]),
                    Rank = Convert.ToInt32(row["rank"]),
                    SpeedST = Convert.ToDouble(row["speed_st"]),
                    HandlingST = Convert.ToDouble(row["handling_st"]),
                    AccelerationST = Convert.ToDouble(row["acceleration_st"]),
                    LaunchST = Convert.ToDouble(row["launch_st"]),
                    BrakingST = Convert.ToDouble(row["braking_st"]),
                    OffroadST = Convert.ToDouble(row["offroad_st"]),
                    TopSpeed = Convert.ToDouble(row["top_speed"]),
                    ZeroToSixty = Convert.ToDouble(row["zero_to_sixty"]),
                    Deleted = Convert.ToInt32(row["deleted"]),
                };
                builds.Add(build);
            }
            return builds;
         }

        public Build GetBuildByID(int id)
        {
            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "SELECT build_id, user_id, car_id, `rank`, speed_st, handling_st, acceleration_st, launch_st, braking_st, offroad_st, top_speed, zero_to_sixty, deleted FROM Builds WHERE build_id = @id";

            using var cmd = new MySqlCommand(stm, con);

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new Build
                {
                    BuildId = Convert.ToInt32(row["build_id"]),
                    UserId = Convert.ToInt32(row["user_id"]),
                    CarId = Convert.ToInt32(row["car_id"]),
                    Rank = Convert.ToInt32(row["rank"]),
                    SpeedST = Convert.ToDouble(row["speed_st"]),
                    HandlingST = Convert.ToDouble(row["handling_st"]),
                    AccelerationST = Convert.ToDouble(row["acceleration_st"]),
                    LaunchST = Convert.ToDouble(row["launch_st"]),
                    BrakingST = Convert.ToDouble(row["braking_st"]),
                    OffroadST = Convert.ToDouble(row["offroad_st"]),
                    TopSpeed = Convert.ToDouble(row["top_speed"]),
                    ZeroToSixty = Convert.ToDouble(row["zero_to_sixty"]),
                    Deleted = Convert.ToInt32(row["deleted"]),
                };
            }

            return null;
        }

         public void CreateBuild(Build build){
            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = @"INSERT INTO Builds(user_id, car_id, `rank`, speed_st, handling_st, acceleration_st, launch_st, braking_st, offroad_st, top_speed, zero_to_sixty, deleted) VALUES(@user_id, @car_id, @rank, @speed_st, @handling_st, @acceleration_st, @launch_st, @braking_st, @offroad_st, @top_speed, @zero_to_sixty, @deleted)";

            using var cmd = new MySqlCommand(stm,con);

            cmd.Parameters.AddWithValue("@buildID", build.BuildId);
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
            cmd.Parameters.AddWithValue("deleted", build.Deleted);

            cmd.Prepare();

            cmd.ExecuteNonQuery();
         }

         public bool UpdateBuild(Build build, int id){
            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = $"UPDATE Builds SET user_id =  user_id, car_id = @car_id, `rank` = @rank, speed_st = @speed_st, handling_st = @handling_st, acceleration_st = @acceleration_st, launch_st = @launch_st, braking_st = @braking_st, offroad_st = @offroad_st, top_speed = @top_speed, zero_to_sixty = @zero_to_sixty WHERE build_id = @id";

            using var cmd = new MySqlCommand(stm, con);
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

            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
         }

         public void DeleteBuild(int id){
            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "DELETE FROM Builds WHERE build_id = @id";

            using var cmd = new MySqlCommand(stm, con);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
         }

        public bool SetBuildDeleted(int id)
        {
            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "UPDATE Builds SET deleted = 1 WHERE build_id = @id";

            using var cmd = new MySqlCommand(stm, con);
            cmd.Parameters.AddWithValue("@id", id);

            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}