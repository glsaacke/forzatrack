using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.models;

namespace api.core.services.UserService
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User GetUserByID(int id);
        void CreateUser(CreateUser user);
        bool UpdateUser(User user, int id);
        void DeleteUser(int id);
        bool SetUserDeleted(int id);
        User GetUserByEmail(string username);
    }
}