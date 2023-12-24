using BusinessLogic.Mapper;
using BusinessLogic.Utils;
using DataAccess.Repository;
using Transversal.Entities;

namespace BusinessLogic.Services
{
    public class UserService
    {
        private UserRepository _userRepository = new UserRepository();
    
        public void CreateUser(UserDto userDto)
        {
            userDto.Password = PasswordHasher.HashPassword(userDto.Password);
            _userRepository.CreateUser(UserMapper.MapToDao(userDto));
        }

        public UserDto UpdateUser(UserDto userDto)
        {
            return UserMapper.MapToDto(_userRepository.UpdateUser(UserMapper.MapToDao(userDto)));
        }

        public UserDto GetUser(string username)
        {
            return UserMapper.MapToDto(_userRepository.GetUserByUsername(username));
        }

        public bool Login(UserDto userDto)
        {
            string password = userDto.Password;
            var dbUser = UserMapper.MapToDto(_userRepository.GetUserByUsername(userDto.Username));
            
            if (dbUser == null)
                return false;

            return PasswordHasher.VerifyPassword(password, dbUser.Password);
        }

        public bool UserExists(string username)
        {
            return _userRepository.UserExists(username);
        }
    }    
}

