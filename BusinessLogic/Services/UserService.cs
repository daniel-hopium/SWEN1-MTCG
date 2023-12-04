using BusinessLogic.Mapper;
using BusinessLogic.Utils;
using DataAccess.Repository;
using Transversal.Entities;

namespace BusinessLogic.Services
{
    public class UserService
    {
        private UserRepository _userRepository = new UserRepository();
    
        public void CreateUser(User user)
        {
            user.Password = PasswordHasher.HashPassword(user.Password);

            _userRepository.CreateUser(UserMapper.MapToDao(user));
        }

        public User UpdateUser(User user)
        {
            return UserMapper.MapToEntity(_userRepository.UpdateUser(UserMapper.MapToDao(user)));
        }

        public User GetUser(string username)
        {
            return UserMapper.MapToEntity(_userRepository.GetUserByUsername(username));
        }

        public bool Login(User user)
        {
            string password = user.Password;
            User dbUser = UserMapper.MapToEntity(_userRepository.GetUserByUsername(user.Username));
            
            if (dbUser.Username == null || dbUser.Password == null)
                return false;

            return PasswordHasher.VerifyPassword(password, dbUser.Password);
            // extend later
        }

        public bool UserExists(string username)
        {
            return _userRepository.UserExists(username);
        }
    }    
}

