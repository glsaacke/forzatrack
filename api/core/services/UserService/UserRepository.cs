using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using api.core.models;
using api.core.data;

namespace api.core.services.UserService
{
    public class UserRepository : IUserRepository
    {
        public List<User> GetAllUsers(){

            List<User> users = new List<User>();

            string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "SELECT user_id, fname, lname, email, password, deleted FROM Users";

            using var cmd = new MySqlCommand(stm, con);

            using var reader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            foreach (DataRow row in dataTable.Rows)
            {
                User user = new User
                {
                    UserId = Convert.ToInt32(row["user_id"]),
                    FName = row["fname"].ToString(),
                    LName = row["lname"].ToString(),
                    Email = row["email"].ToString(),
                    Password = row["password"].ToString(),
                    Deleted = Convert.ToInt32(row["deleted"]),
                };
                users.Add(user);
            }
            return users;
        }

        public User GetUserByID(int id){
            string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "SELECT user_id, fname, lname, email, password, deleted FROM Users WHERE user_id = @id";

            using var cmd = new MySqlCommand(stm, con);

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new User
                {
                    UserId = Convert.ToInt32(row["user_id"]),
                    FName = row["fname"].ToString(),
                    LName = row["lname"].ToString(),
                    Email = row["email"].ToString(),
                    Password = row["password"].ToString(),
                    Deleted = Convert.ToInt32(row["deleted"]),
                };
            }

            return null;
         }

        public void CreateUser(User user){
            string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = @"INSERT INTO Users(user_id, fname, lname, email, password, deleted) VALUES(@user_id, @fname, @lname, @email, @password, @deleted)";

            using var cmd = new MySqlCommand(stm,con);

            cmd.Parameters.AddWithValue("@user_id", user.UserId);
            cmd.Parameters.AddWithValue("@fname", user.FName);
            cmd.Parameters.AddWithValue("@lname", user.LName);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@deleted", user.Deleted);

            cmd.Prepare();

            cmd.ExecuteNonQuery();
         }

        public void UpdateUser(User user, int id){
            string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = $"UPDATE Users SET fname = @fname, lname = @lname, email = @email, password = @password WHERE user_id = @id";

            using var cmd = new MySqlCommand(stm, con);
            cmd.Parameters.AddWithValue("@fname", user.FName);
            cmd.Parameters.AddWithValue("@lname", user.LName);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
         }

        public void DeleteUser(int id){
            string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "UPDATE Users SET deleted = 1 WHERE user_id = @id";

            using var cmd = new MySqlCommand(stm, con);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
         }
    }
}