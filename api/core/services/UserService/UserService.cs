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
                        Username = user.Username,
                        Email = email
                    }};
                }
                else{
                    return new AuthResponse{ Success = false, Message="Incorrect Password"};
                }
            }
        }

        public AuthResponse CreateUser(CreateUser user)
        {
            User emailUser = userRepository.GetUserByEmail(user.Email);
            User usernameUser = userRepository.GetUserByUsername(user.Username);

            if (emailUser != null){
                return new AuthResponse{ Success = false, Message = "An account with this email already exists"};
            }
            else if (usernameUser != null){
                return new AuthResponse{Success = false, Message="Username taken"};
            }
            else{
                userRepository.CreateUser(user);
                User newUser = userRepository.GetUserByEmail(user.Email);
                return new AuthResponse{ Success = true, Message="", User = new UserDto{
                    UserId = newUser.UserId,
                    Username = user.Username,
                    Email = newUser.Email
                }};
            }
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