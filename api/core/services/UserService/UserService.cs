using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.models;
using api.core.models.responses;

namespace api.core.services.UserService
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository;
        public UserService(IUserRepository userRepository){
            this.userRepository = userRepository;
        }

        public AuthResponse AuthenticateUser(string email, string password)
        {
            User user = userRepository.GetUserByEmail(email);

            if(user == null){
                return new AuthResponse{ Success = false, Message=$"No accounts matching '{email}'"};
            }
            else{
                if(user.Password == password && user.Deleted == 0){
                    return new AuthResponse{ Success = true, Message="Login Successful", User = new UserDto{
                        UserId = user.UserId,
                        FName = user.FName,
                        LName = user.LName,
                        Email = email
                    }};
                }
                else{
                    return new AuthResponse{ Success = false, Message="Incorrect Password"};
                }
            }
        }

        public void CreateUser(CreateUser user)
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

        public bool SetUserDeleted(int id)
        {
            bool rowsAffected = userRepository.SetUserDeleted(id);
            return rowsAffected;
        }

        public bool UpdateUser(User user, int id)
        {
            bool rowsAffected = userRepository.UpdateUser(user, id);
            return rowsAffected;
        }
    }
}