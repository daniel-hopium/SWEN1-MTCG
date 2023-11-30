using TradingCardGame.NET.persistence.entity;
using TradingCardGame.NET.persistence.repository;

namespace TradingCardGame.NET.service;

public class UserService
{

    private UserRepository _userRepository = new UserRepository();
    
    public void CreateUser(User user)
    {
        _userRepository.AddUser(user);
    }

    public User UpdateUser(User user)
    {
        return _userRepository.UpdateUser(user);
    }

    public User GetData(string username)
    {
        return _userRepository.GetUserByUsername(username);
    }
}