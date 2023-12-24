using DataAccess.Daos;
using Transversal.Entities;

namespace BusinessLogic.Mapper;

public class UserMapper
{
    public static UserDto MapToDto(UserDao userDao)
    {
        if (userDao == null)
        {
            return null;
        }

        return new UserDto
        {
            Username = userDao.Username,
            Name = userDao.Name,
            Bio = userDao.Bio,
            Image = userDao.Image
        };
    }
    
    public static UserDto MapToDtoWithStats(UserDao userDao)
    {
        if (userDao == null)
        {
            return null;
        }

        return new UserDto
        {
            Username = userDao.Username,
            Bio = userDao.Bio,
            Image = userDao.Image,
        };
    }
    
    public static UserDao MapToDao(UserDto userDto)
    {
        if (userDto == null)
        {
            return null;
        }

        return new UserDao
        {
            Username = userDto.Username,
            Password = userDto.Password,
            Name = userDto.Name,
            Bio = userDto.Bio,
            Image = userDto.Image
        };
    }
    public static List<UserDto> MapToDto(List<UserDao> userDaos)
    {
        if (userDaos == null)
        {
            return null;
        }

        return userDaos.Select(MapToDto).ToList();
    }
}
