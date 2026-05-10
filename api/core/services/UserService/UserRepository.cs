using api.core.models;
using api.core.data;
using Microsoft.EntityFrameworkCore;

namespace api.core.services.UserService
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context){
            _context = context;
        }

        public List<User> GetAllUsers(){
            return _context.Users.ToList();
        }

        public User GetUserByID(int id){
            return _context.Users.FirstOrDefault(u => u.UserId == id);
        }

        public void CreateUser(CreateUser user){
            var newUser = new User
            {
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,
                Deleted = user.Deleted,
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public bool UpdateUser(User user, int id){
            var existing = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (existing == null) return false;

            existing.Username = user.Username;
            existing.Email = user.Email;
            existing.Password = user.Password;
            return _context.SaveChanges() > 0;
        }

        public void DeleteUser(int id){
            var user = new User { UserId = id };
            _context.Users.Attach(user);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public bool SetUserDeleted(int id){
            var existing = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (existing == null) return false;

            existing.Deleted = 1;
            return _context.SaveChanges() > 0;
        }

        public User GetUserByEmail(string email){
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetUserByUsername(string username){
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }
    }
}