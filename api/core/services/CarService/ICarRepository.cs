using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.models;

namespace api.core.services.CarService
{
    public interface ICarRepository
    {
        List<Car> GetAllCars();
        Car GetCarByID(int id);
        void CreateCar(Car car);
        bool UpdateCar(Car car, int id);
        void DeleteCar(int id);
        bool SetCarDeleted(int id);
    }
}