using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using api.core.models;
using api.core.data;

namespace api.core.services.CarService
{
    public class CarRepository : ICarRepository
    {
         public List<Car> GetAllCars(){
            List<Car> cars = new List<Car>();

            string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "SELECT car_id, make, model, year, deleted FROM Cars";

            using var cmd = new MySqlCommand(stm, con);

            using var reader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            foreach (DataRow row in dataTable.Rows)
            {
                Car car = new Car
                {
                    CarId = Convert.ToInt32(row["car_id"]),
                    Make = row["make"].ToString(),
                    Model = row["model"].ToString(),
                    Year = Convert.ToInt32(row["year"]),
                    Deleted = Convert.ToInt32(row["deleted"]),
                };
                cars.Add(car);
            }
            return cars;
         }
        public Car GetCarByID(int id){
            string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "SELECT car_id, make, model, year, deleted FROM Cars WHERE car_id = @id";

            using var cmd = new MySqlCommand(stm, con);

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new Car
                {
                    CarId = Convert.ToInt32(row["car_id"]),
                    Make = row["make"].ToString(),
                    Model = row["model"].ToString(),
                    Year = Convert.ToInt32(row["year"]),
                    Deleted = Convert.ToInt32(row["deleted"]),
                };
            }

            return null;
         }


         public void CreateCar(Car car){
            string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = @"INSERT INTO Cars(make, model, year, deleted) VALUES(@make, @model, @year, @deleted)";

            using var cmd = new MySqlCommand(stm,con);

            cmd.Parameters.AddWithValue("@make", car.Make);
            cmd.Parameters.AddWithValue("@model", car.Model);
            cmd.Parameters.AddWithValue("@year", car.Year);
            cmd.Parameters.AddWithValue("deleted", car.Deleted);

            cmd.Prepare();

            cmd.ExecuteNonQuery();
         }

         public void UpdateCar(Car car){
            string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = $"UPDATE Cars SET make = @make, model = @model, year = @year, WHERE car_id = @id";

            using var cmd = new MySqlCommand(stm, con);
            cmd.Parameters.AddWithValue("@make", car.Make);
            cmd.Parameters.AddWithValue("@model", car.Model);
            cmd.Parameters.AddWithValue("@year", car.Year);
            cmd.Parameters.AddWithValue("@id", car.CarId);

            cmd.ExecuteNonQuery();
         }

         public void DeleteCar(int id){
            string cs = Connection.conString;
            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = "UPDATE Cars SET deleted = 1 WHERE car_id = @id";

            using var cmd = new MySqlCommand(stm, con);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
         }
    }
}