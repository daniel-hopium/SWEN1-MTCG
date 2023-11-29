using TradingCardGame.NET.persistence.entity;
using TradingCardGame.NET.persistence.repository;

namespace TradingCardGame.NET.service;

public class UserService
{

    private UserRepository _userRepository = new UserRepository("Host=127.0.0.1;Port=5432;Username=postgres;Password=postgres;Database=postgres");
    
    
    public void CreateUser(User user)
    {
        Console.WriteLine("User Created:");
        Console.WriteLine("Username: " + user.username);
        Console.WriteLine("Password: " + user.password);
        _userRepository.AddUser(user);
    }

    public void UpdateUser(User user)
    {
        Console.WriteLine(user.ToString());
        //throw new NotImplementedException();
    }

    public void GetData(string username)
    {
        Console.WriteLine(username);
        //throw new NotImplementedException();
    }
}