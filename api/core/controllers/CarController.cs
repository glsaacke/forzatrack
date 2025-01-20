using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.core.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController
    {
        [HttpGet("GetAllCars")]
        public IEnumerable<string> GetAllCars()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("GetCarById/{id}")]
        public string GetCarById(int id)
        {
            return "value";
        }

        [HttpPost("CreateCar")]
        public void CreateCar([FromBody] string value)
        {
        }

        [HttpPut("UpdateCar/{id}")]
        public void UpdateCar(int id, [FromBody] string value)
        {
        }

        [HttpDelete("DeleteCar/{id}")]
        public void DeleteCar(int id)
        {
        }
    }
}