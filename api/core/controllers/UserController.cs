using Microsoft.AspNetCore.Mvc;
using api.core.controllers.models;
using api.core.models;
using api.core.models.responses;
using api.core.services.UserService;

namespace api.core.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            if (users == null || users.Count == 0)
                return NotFound(new { Message = "No users found." });

            return Ok(users);
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound(new { Message = "No user found matching the id." });

            return Ok(user);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest request)
        {
            var user = new CreateUser
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password,
            };

            var response = await _userService.CreateUserAsync(user);
            return Ok(response);
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequest request)
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password,
            };

            var updated = await _userService.UpdateUserAsync(user, id);
            return updated ? Ok() : NotFound("No user found matching the id.");
        }

        [HttpPut("SetUserDeleted/{id}")]
        public async Task<IActionResult> SetUserDeleted(int id)
        {
            var updated = await _userService.SetUserDeletedAsync(id);
            return updated ? Ok() : NotFound("No user found matching the id.");
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok();
        }

        [HttpGet("AuthenticateUser")]
        public async Task<IActionResult> AuthenticateUser(string email, string password)
        {
            var response = await _userService.AuthenticateUserAsync(email, password);
            return Ok(response);
        }
    }
}