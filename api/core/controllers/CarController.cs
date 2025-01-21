using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.core.controllers.models;
using api.core.models;
using api.core.services.CarService;
using Microsoft.Extensions.Logging;

namespace api.core.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private ICarService carService;
        private ILogger<CarController> logger;
        public CarController(ICarService carService, ILogger<CarController> logger){
            this.carService = carService;
            this.logger = logger;
        }

        [HttpGet("GetAllCars")]
        public IActionResult GetAllCars()
        {
            try{
                var cars = carService.GetAllCars();

                if (cars == null || cars.Count == 0)
                {
                    logger.LogWarning("No cars found.");
                    return NotFound(new { Message = "No cars found." });
                } else {
                    return Ok(cars);
                }
            }
            catch(Exception ex){
                logger.LogError(ex, "An error occurred while fetching all cars.");
                throw;
            }
        }

        [HttpGet("GetCarById/{id}")]
        public IActionResult GetCarById(int id)
        {
            Car car;
            try{
                car = carService.GetCarByID(id);

                if (car == null){
                    logger.LogWarning("No cars found.");
                    return NotFound(new { Message = "No cars found." });
                } else {
                    return Ok(car);
                }
            }
            catch(Exception ex){
                logger.LogError(ex, "An error occurred while fetching all users.");
                throw;
            }
        }

        [HttpPost("CreateCar")]
        public IActionResult CreateCar([FromBody] CarRequest request)
        {
            if(request == null){
                logger.LogError("The request was null");
                return BadRequest("Request body cannot be null.");
            }
            else{
                Car car;
                try{

                    car = new Car{
                        Make = request.Make,
                        Model = request.Model,
                        Year = request.Year,
                        Deleted = request.Deleted
                    };

                    carService.CreateCar(car);

                    return Ok();
                }
                catch(Exception ex){
                    logger.LogError(ex, "An error occurred while updating car.");
                    throw;
                }
            }
        }

        [HttpPut("UpdateCar/{id}")]
        public IActionResult UpdateCar(int id, [FromBody] CarRequest request)
        {
             if(request == null){
                logger.LogError("The request was null");
                return BadRequest("Request body cannot be null.");
            }
            else{
                Car car;
                try{

                    car = new Car{
                        Make = request.Make,
                        Model = request.Model,
                        Year = request.Year,
                        Deleted = request.Deleted
                    };

                    carService.UpdateCar(car, id);

                    return Ok();
                }
                catch(Exception ex){
                    logger.LogError(ex, "An error occurred while updating car.");
                    throw;
                }
            }
        }

        [HttpDelete("DeleteCar/{id}")]
        public IActionResult DeleteCar(int id)
        {
            try{
                carService.DeleteCar(id);
                return Ok();
            }
            catch(Exception ex){
                logger.LogError(ex, "An error occurred while deleting user.");
                throw;
            }
        }
    }
}