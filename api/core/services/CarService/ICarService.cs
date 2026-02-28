using api.core.models;

namespace api.core.services.CarService
{
    public interface ICarService
    {
        Task<List<Car>> GetAllCarsAsync();
        Task<Car?> GetCarByIdAsync(int id);
        Task CreateCarAsync(Car car);
        Task<bool> UpdateCarAsync(Car car, int id);
        Task DeleteCarAsync(int id);
        Task<bool> SetCarDeletedAsync(int id);
    }
}