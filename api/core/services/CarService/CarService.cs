using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.models;

namespace api.core.services.CarService
{
    public class CarService : ICarService
    {
        private ICarRepository carRepository;
        public CarService(ICarRepository carRepository){
            this.carRepository = carRepository;
        }

        public void CreateCar(Car car)
        {
            carRepository.CreateCar(car);
        }

        public void DeleteCar(int id)
        {
            carRepository.DeleteCar(id);
        }

        public List<Car> GetAllCars()
        {
            var cars = carRepository.GetAllCars();
            return cars;
        }

        public Car GetCarByID(int id)
        {
            Car car = carRepository.GetCarByID(id);
            return car;
        }

        public bool SetCarDeleted(int id)
        {
            bool rowsAffected = carRepository.SetCarDeleted(id);
            return rowsAffected;
        }

        public bool UpdateCar(Car car, int id)
        {
            bool rowsAffected = carRepository.UpdateCar(car, id);
            return rowsAffected;
        }
    }
}