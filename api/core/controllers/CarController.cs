using Microsoft.AspNetCore.Mvc;
using api.core.controllers.models;
using api.core.models;
using api.core.services.CarService;

namespace api.core.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet("GetAllCars")]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _carService.GetAllCarsAsync();

            if (cars == null || cars.Count == 0)
                return NotFound(new { Message = "No cars found." });

            return Ok(cars);
        }

        [HttpGet("GetCarById/{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);

            if (car == null)
                return NotFound(new { Message = "No car found matching the id." });

            return Ok(car);
        }

        [HttpPost("CreateCar")]
        public async Task<IActionResult> CreateCar([FromBody] CarRequest request)
        {
            var car = new Car
            {
                Make = request.Make,
                Model = request.Model,
                Year = request.Year,
            };

            await _carService.CreateCarAsync(car);
            return Ok();
        }

        [HttpPut("UpdateCar/{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] CarRequest request)
        {
            var car = new Car
            {
                Make = request.Make,
                Model = request.Model,
                Year = request.Year,
            };

            var updated = await _carService.UpdateCarAsync(car, id);
            return updated ? Ok() : NotFound("No car found matching the id.");
        }

        [HttpPut("SetCarDeleted/{id}")]
        public async Task<IActionResult> SetCarDeleted(int id)
        {
            var updated = await _carService.SetCarDeletedAsync(id);
            return updated ? Ok() : NotFound("No car found matching the id.");
        }

        [HttpDelete("DeleteCar/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            await _carService.DeleteCarAsync(id);
            return Ok();
        }
    }
}