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
    public class UserController
    {
         [HttpGet("GetAllUsers")]
        public IEnumerable<string> GetAllUsers()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("GetUserById/{id}")]
        public string GetUserById(int id)
        {
            return "value";
        }

        [HttpPost("CreateUser")]
        public void CreateUser([FromBody] string value)
        {
        }

        [HttpPut("UpdateUser/{id}")]
        public void UpdateUser(int id, [FromBody] string value)
        {
        }

        [HttpDelete("DeleteUser/{id}")]
        public void DeleteUser(int id)
        {
        }
    }
}