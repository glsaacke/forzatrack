using api.core.models;

namespace api.core.services.CarService
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task CreateCarAsync(Car car)
        {
            await _carRepository.CreateCarAsync(car);
        }

        public async Task DeleteCarAsync(int id)
        {
            await _carRepository.DeleteCarAsync(id);
        }

        public async Task<List<Car>> GetAllCarsAsync()
        {
            var cars = await _carRepository.GetAllCarsAsync();
            return cars.OrderBy(c => c.Make).ThenByDescending(c => c.Year).ThenBy(c => c.Model).ToList();
        }

        public async Task<Car?> GetCarByIdAsync(int id)
        {
            return await _carRepository.GetCarByIdAsync(id);
        }

        public async Task<bool> SetCarDeletedAsync(int id)
        {
            return await _carRepository.SetCarDeletedAsync(id);
        }

        public async Task<bool> UpdateCarAsync(Car car, int id)
        {
            return await _carRepository.UpdateCarAsync(car, id);
        }
    }
}