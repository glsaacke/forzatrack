using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.models;
using api.core.models.responses;

namespace api.core.services.UserService
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User GetUserByID(int id);
        void CreateUser(CreateUser user);
        bool UpdateUser(User user, int id);
        void DeleteUser(int id);
        bool SetUserDeleted(int id);
        AuthResponse AuthenticateUser(string email, string password);
    }
}