using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.core.controllers.models;
using api.core.models;
using api.core.services.UserService;
using Microsoft.Extensions.Logging;

namespace api.core.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService userService;
        private ILogger<UserController> logger;
        public UserController(IUserService userService, ILogger<UserController> logger){
            this.userService = userService;
            this.logger = logger;
        }

        [HttpGet("GetAllUsers")] //FIXME resolve DI issue
        public IActionResult GetAllUsers()
        {
            try{
                var users = userService.GetAllUsers();

                if (users == null || users.Count == 0)
                {
                    logger.LogWarning("No users found.");
                    return NotFound(new { Message = "No users found." });
                } else {
                    return Ok(users);
                }
            }
            catch(Exception ex){
                logger.LogError(ex, "An error occurred while fetching all users.");
                throw;
            }   
        }

        [HttpGet("GetUserById/{id}")] //TODO finish endpoints
        public IActionResult GetUserById(int id)
        {
            User user;
            try{
                user = userService.GetUserByID(id);

                if (user == null){
                    logger.LogWarning("No users found.");
                    return NotFound(new { Message = "No users found." });
                } else {
                    return Ok(user);
                }
            }
            catch(Exception ex){
                logger.LogError(ex, "An error occurred while fetching all users.");
                throw;
            }
        }

        [HttpPost("CreateUser")]
        public void CreateUser([FromBody] UserRequest request)
        {
            User user = new User{
                FName = request.FName,
                LName = request.LName,
                Email = request.Email,
                Password = request.Password,
                Deleted = request.Deleted
            };

            userService.CreateUser(user);
        }

        [HttpPut("UpdateUser/{id}")]
        public void UpdateUser(int id, [FromBody] UserRequest request)
        {
            User user = new User{
                FName = request.FName,
                LName = request.LName,
                Email = request.Email,
                Password = request.Password,
                Deleted = request.Deleted
            };
            
            userService.UpdateUser(user);
        }

        [HttpDelete("DeleteUser/{id}")]
        public void DeleteUser(int id)
        {
            userService.DeleteUser(id);
        }
    }
}