using api.core.models;

namespace api.core.services.UserService
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task CreateUserAsync(CreateUser user);
        Task<bool> UpdateUserAsync(User user, int id);
        Task DeleteUserAsync(int id);
        Task<bool> SetUserDeletedAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUsernameAsync(string username);
    }
}