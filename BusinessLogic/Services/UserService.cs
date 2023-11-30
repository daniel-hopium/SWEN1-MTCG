using BusinessLogic.Mapper;
using DataAccess.Repository;
using Transversal.Entities;

namespace BusinessLogic.Services
{
    public class UserService
    {
        private UserRepository _userRepository = new UserRepository();
    
        public void CreateUser(User user)
        {
                //date, password hashing
            _userRepository.AddUser(UserMapper.MapToDao(user));
            
        }

        public User UpdateUser(User user)
        {
            return UserMapper.MapToEntity(_userRepository.UpdateUser(UserMapper.MapToDao(user)));
        }

        public User GetData(string username)
        {
            return UserMapper.MapToEntity(_userRepository.GetUserByUsername(username));
        }
    }    
}

