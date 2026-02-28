using api.core.models;
using api.core.models.responses;

namespace api.core.services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AuthResponse> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return new AuthResponse { Success = false, Message = $"No accounts matching '{email}'" };
            }

            if (user.Password == password && user.Deleted == 0)
            {
                return new AuthResponse
                {
                    Success = true,
                    Message = "Login Successful",
                    User = new UserDto
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        Email = user.Email
                    }
                };
            }

            return new AuthResponse { Success = false, Message = "Incorrect Password" };
        }

        public async Task<AuthResponse> CreateUserAsync(CreateUser user)
        {
            var emailUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (emailUser != null)
            {
                return new AuthResponse { Success = false, Message = "An account with this email already exists" };
            }

            var usernameUser = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (usernameUser != null)
            {
                return new AuthResponse { Success = false, Message = "Username taken" };
            }

            await _userRepository.CreateUserAsync(user);
            var newUser = await _userRepository.GetUserByEmailAsync(user.Email);

            return new AuthResponse
            {
                Success = true,
                User = new UserDto
                {
                    UserId = newUser!.UserId,
                    Username = newUser.Username,
                    Email = newUser.Email
                }
            };
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<bool> SetUserDeletedAsync(int id)
        {
            return await _userRepository.SetUserDeletedAsync(id);
        }

        public async Task<bool> UpdateUserAsync(User user, int id)
        {
            return await _userRepository.UpdateUserAsync(user, id);
        }
    }
}