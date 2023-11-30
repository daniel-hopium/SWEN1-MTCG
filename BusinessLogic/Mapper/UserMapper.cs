using DataAccess.Daos;
using Transversal.Entities;

namespace BusinessLogic.Mapper;

public class UserMapper
{
    public static User MapToEntity(UserDao userDao)
    {
        if (userDao == null)
        {
            return null;
        }

        return new User
        {
            Username = userDao.Username,
            Password = userDao.Password,
            Name = userDao.Name,
            Bio = userDao.Bio,
            Image = userDao.Image
        };
    }
    
    public static UserDao MapToDao(User user)
    {
        if (user == null)
        {
            return null;
        }

        return new UserDao
        {
            Username = user.Username,
            Password = user.Password,
            Name = user.Name,
            Bio = user.Bio,
            Image = user.Image
        };
    }
}
