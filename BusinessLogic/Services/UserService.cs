using System.Security.Authentication;
using BusinessLogic.Mapper;
using BusinessLogic.Utils;
using DataAccess.Repository;
using Transversal.Entities;

namespace BusinessLogic.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository = new UserRepository();
    
        public void CreateUser(UserDto userDto)
        {
            userDto.Password = PasswordHasher.HashPassword(userDto.Password!);
            var userId = _userRepository.CreateUser(UserMapper.MapToDao(userDto));
            _userRepository.CreateUserScore(userId);
        }

        public UserDto UpdateUser(string oldUsername, UserDto userDto)
        {
            try
            {
                var oldUser = _userRepository.GetUserByUsername(oldUsername)!;
                if(oldUser == null)
                    throw new InvalidCredentialException("User not found");
                oldUser.Name = userDto.Name; 
                oldUser.Bio = userDto.Bio;
                oldUser.Image = userDto.Image;
                
                return UserMapper.MapToDto(_userRepository.UpdateUser(oldUser));
            }
            catch (InvalidCredentialException e)
            {
                Console.WriteLine(e);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Error updating user");
            }
        }

        public UserDto GetUser(string username)
        {
            return UserMapper.MapToDto(_userRepository.GetUserByUsername(username)!);
        }

        public bool Login(UserDto userDto)
        {
            string password = userDto.Password!;
            var dbUser = UserMapper.MapToDto(_userRepository.GetUserByUsername(userDto.Username!)!);
            
            if (dbUser == null)
                return false;

            return PasswordHasher.VerifyPassword(password, dbUser.Password!);
        }

        public bool UserExists(string username)
        {
            return _userRepository.UserExists(username);
        }
    }    
}

