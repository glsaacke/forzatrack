using api.core.models;
using api.core.models.responses;

namespace api.core.services.UserService
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<AuthResponse> CreateUserAsync(CreateUser user);
        Task<bool> UpdateUserAsync(User user, int id);
        Task DeleteUserAsync(int id);
        Task<bool> SetUserDeletedAsync(int id);
        Task<AuthResponse> AuthenticateUserAsync(string email, string password);
    }
}