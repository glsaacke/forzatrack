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
        private readonly string cs;
        public UserRepository(string cs){
            this.cs = cs;
        }

        public List<User> GetAllUsers(){

            List<User> users = new List<User>();

            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "SELECT user_id, username, email, password, deleted FROM Users";

            using var cmd = new MySqlCommand(stm, con);

            using var reader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            foreach (DataRow row in dataTable.Rows)
            {
                User user = new User
                {
                    UserId = Convert.ToInt32(row["user_id"]),
                    Username = row["username"].ToString(),
                    Email = row["email"].ToString(),
                    Password = row["password"].ToString(),
                    Deleted = Convert.ToInt32(row["deleted"]),
                };
                users.Add(user);
            }
            return users;
        }

        public User GetUserByID(int id){
            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "SELECT user_id, username, email, password, deleted FROM Users WHERE user_id = @id";

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
                    Username = row["username"].ToString(),
                    Email = row["email"].ToString(),
                    Password = row["password"].ToString(),
                    Deleted = Convert.ToInt32(row["deleted"]),
                };
            }

            return null;
         }

        public void CreateUser(CreateUser user){
            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = @"INSERT INTO Users(username, email, password, deleted) VALUES(@username, @email, @password, @deleted)";

            using var cmd = new MySqlCommand(stm,con);

            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@deleted", user.Deleted);

            cmd.Prepare();

            cmd.ExecuteNonQuery();
         }

        public bool UpdateUser(User user, int id){
            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = $"UPDATE Users SET username = @username, email = @email, password = @password WHERE user_id = @id";

            using var cmd = new MySqlCommand(stm, con);
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@id", id);

            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
         }

        public void DeleteUser(int id){
            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "DELETE FROM Users WHERE user_id = @id";

            using var cmd = new MySqlCommand(stm, con);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
         }

         public bool SetUserDeleted(int id){
            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "UPDATE Users SET deleted = 1 WHERE user_id = @id";

            using var cmd = new MySqlCommand(stm, con);
            cmd.Parameters.AddWithValue("@id", id);

            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
         }

        public User GetUserByEmail(string email)
        {
            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "SELECT user_id, username, email, password, deleted FROM Users WHERE email = @email";

            using var cmd = new MySqlCommand(stm, con);

            cmd.Parameters.AddWithValue("@email", email);

            using var reader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new User
                {
                    UserId = Convert.ToInt32(row["user_id"]),
                    Username = row["username"].ToString(),
                    Email = row["email"].ToString(),
                    Password = row["password"].ToString(),
                    Deleted = Convert.ToInt32(row["deleted"]),
                };
            }

            return null;
        }

        public User GetUserByUsername(string username)
        {
            // string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "SELECT user_id, username, email, password, deleted FROM Users WHERE username = @username";

            using var cmd = new MySqlCommand(stm, con);

            cmd.Parameters.AddWithValue("@username", username);

            using var reader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new User
                {
                    UserId = Convert.ToInt32(row["user_id"]),
                    Username = row["username"].ToString(),
                    Email = row["email"].ToString(),
                    Password = row["password"].ToString(),
                    Deleted = Convert.ToInt32(row["deleted"]),
                };
            }

            return null;
        }
    }
}