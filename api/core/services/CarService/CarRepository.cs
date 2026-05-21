using api.core.models;
using api.core.data;

namespace api.core.services.CarService
{
    public class CarRepository : ICarRepository
    {
        private readonly AppDbContext _context;
        public CarRepository(AppDbContext context){
            _context = context;
        }

        public List<Car> GetAllCars(){
            return _context.Cars.ToList();
        }

        public Car GetCarByID(int id){
            return _context.Cars.FirstOrDefault(c => c.CarId == id);
        }

        public void CreateCar(Car car){
            _context.Cars.Add(car);
            _context.SaveChanges();
        }

        public bool UpdateCar(Car car, int id){
            var existing = _context.Cars.FirstOrDefault(c => c.CarId == id);
            if (existing == null) return false;

            existing.Make = car.Make;
            existing.Model = car.Model;
            existing.Year = car.Year;
            return _context.SaveChanges() > 0;
        }

        public void DeleteCar(int id){
            var car = new Car { CarId = id };
            _context.Cars.Attach(car);
            _context.Cars.Remove(car);
            _context.SaveChanges();
        }

        public bool SetCarDeleted(int id){
            var existing = _context.Cars.FirstOrDefault(c => c.CarId == id);
            if (existing == null) return false;

            existing.Deleted = 1;
            return _context.SaveChanges() > 0;
        }
    }
}
