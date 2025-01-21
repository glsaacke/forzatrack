using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.models;

namespace api.core.services.CarService
{
    public interface ICarService
    {
        List<Car> GetAllCars();
        Car GetCarByID(int id);
        void CreateCar(Car car);
        void UpdateCar(Car car, int id);
        void DeleteCar(int id);
    }
}