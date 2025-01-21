using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.models;

namespace api.core.services.UserService
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository;
        public UserService(IUserRepository userRepository){
            this.userRepository = userRepository;
        }

        public void CreateUser(User user)
        {
            userRepository.CreateUser(user);
        }

        public void DeleteUser(int id)
        {
            userRepository.DeleteUser(id);
        }

        public List<User> GetAllUsers()
        {
            var users = userRepository.GetAllUsers();
            return users;
        }

        public User GetUserByID(int id)
        {
            User user = userRepository.GetUserByID(id);
            return user;
        }

        public void UpdateUser(User user, int id)
        {
            userRepository.UpdateUser(user, id);
        }
    }
}