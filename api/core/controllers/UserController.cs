using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.core.controllers.models;
using api.core.models;
using api.core.models.responses;
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

        [HttpGet("GetAllUsers")]
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

        [HttpGet("GetUserById/{id}")]
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
                logger.LogError(ex, "An error occurred while fetching user.");
                throw;
            }
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] UserRequest request)
        {
            if(request == null){
                logger.LogError("The request was null");
                return BadRequest("Request body cannot be null.");
            }
            else{
                CreateUser user;
                try{

                    user = new CreateUser{
                        Username = request.Username,
                        Email = request.Email,
                        Password = request.Password,
                        Deleted = request.Deleted,
                    };

                    AuthResponse response = userService.CreateUser(user);

                    return Ok(response);
                }
                catch(Exception ex){
                    logger.LogError(ex, "An error occurred while creating user.");
                    throw;
                }
            }
        }

        [HttpPut("UpdateUser/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserRequest request)
        {
            if(request == null){
                logger.LogError("The request was null");
                return BadRequest("Request body cannot be null.");
            }
            else{
                User user;
                try{

                    user = new User{
                        Username = request.Username,
                        Email = request.Email,
                        Password = request.Password,
                        Deleted = request.Deleted
                    };

                    bool rowsAffected = userService.UpdateUser(user, id);
                    if(rowsAffected){
                        return Ok();
                    } else {
                        return NotFound("No Users found matching the id.");
                    }
                }
                catch(Exception ex){
                    logger.LogError(ex, "An error occurred while updating user.");
                    throw;
                }
            }
        }

        [HttpPut("SetUserDeleted/{id}")]
        public IActionResult SetBuildDeleted(int id)
        {
            try{
                bool rowsAffected = userService.SetUserDeleted(id);
                if(rowsAffected){
                    return Ok();
                } else {
                    return NotFound("No Users found matching the id.");
                }
            }
            catch(Exception ex){
                logger.LogError(ex, "An error occured while setting User Deleted");
                throw;
            }
        }

        [HttpDelete("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            try{
                userService.DeleteUser(id);
                return Ok();
            }
            catch(Exception ex){
                logger.LogError(ex, "An error occurred while deleting user.");
                throw;
            }
        }

        [HttpGet("AuthenticateUser")]
        public IActionResult AuthenticateUser(string email, string password)
        {
            AuthResponse response;
            try{
                response = userService.AuthenticateUser(email, password);
                return Ok(response);
            }
            catch(Exception ex){
                logger.LogError(ex, "An error occurred while authenticating user.");
                throw;
            }
        }

        [HttpPost("RequestPasswordReset")]
        public IActionResult RequestPasswordReset(string email){
            try{
                response = userService.RequestPasswordReset(email);
                return Ok(response);
            }
            catch(Exception ex){
                logger.LogError(ex, "An error occurred while processing request.");
                throw;
            }
        }

        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword(){
            try{
                response = userService.ResetPassword();
                return Ok(response);
            }
            catch(Exception ex){
                logger.LogError(ex, "An error occurred while reseting password.");
                throw;
            }
        }
    }
}