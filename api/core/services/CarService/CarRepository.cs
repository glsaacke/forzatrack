using MySql.Data.MySqlClient;
using api.core.models;

namespace api.core.services.CarService
{
    public class CarRepository : ICarRepository
    {
        private readonly string _connectionString;

        public CarRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Car>> GetAllCarsAsync()
        {
            var cars = new List<Car>();

            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "SELECT car_id, make, model, year, deleted FROM Cars";
            using var cmd = new MySqlCommand(sql, con);
            using var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                cars.Add(MapCar(reader));
            }

            return cars;
        }

        public async Task<Car?> GetCarByIdAsync(int id)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "SELECT car_id, make, model, year, deleted FROM Cars WHERE car_id = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapCar(reader) : null;
        }

        public async Task CreateCarAsync(Car car)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "INSERT INTO Cars(make, model, year, deleted) VALUES(@make, @model, @year, 0)";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@make", car.Make);
            cmd.Parameters.AddWithValue("@model", car.Model);
            cmd.Parameters.AddWithValue("@year", car.Year);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> UpdateCarAsync(Car car, int id)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "UPDATE Cars SET make = @make, model = @model, year = @year WHERE car_id = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@make", car.Make);
            cmd.Parameters.AddWithValue("@model", car.Model);
            cmd.Parameters.AddWithValue("@year", car.Year);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task DeleteCarAsync(int id)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "DELETE FROM Cars WHERE car_id = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> SetCarDeletedAsync(int id)
        {
            using var con = new MySqlConnection(_connectionString);
            await con.OpenAsync();

            const string sql = "UPDATE Cars SET deleted = 1 WHERE car_id = @id";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        private static Car MapCar(MySqlDataReader reader) => new()
        {
            CarId = reader.GetInt32("car_id"),
            Make = reader.GetString("make"),
            Model = reader.GetString("model"),
            Year = reader.GetInt32("year"),
            Deleted = reader.GetInt32("deleted"),
        };
    }
}