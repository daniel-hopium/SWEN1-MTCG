using TradingCardGame.NET.persistence.entity;
using TradingCardGame.NET.service;

namespace TradingCardGame.NET.Controller;

public class UserController
{
    private UserService _userService = new UserService();
    
    public void ProcessRequest(object sender, HttpSvrEventArgs e)
    {

        if (e.Path.Equals("/users") && e.Method.Equals("POST"))
        {
            CreateUser(e);
        }
        else if (e.Path.StartsWith("/users/") && e.Method.Equals("GET"))
        {
            GetUserData(e);
        }
        else if (e.Path.StartsWith("/users/") && e.Method.Equals("PUT"))
        {
            UpdateUserData(e);
        }
    }

    private void UpdateUserData(HttpSvrEventArgs e)
    {
        e.Reply(200, "Successfully updated data");
    }

    private void GetUserData(HttpSvrEventArgs e)
    {
        
        e.Reply(200, "DATA");
    }

    private void CreateUser(HttpSvrEventArgs e)
    {
        User user = new User();
        
        user.username = "testUsername";
        user.password = "wdawdawadwag3r433";
        _userService.CreateUser(user);
        e.Reply(201, "User successfully created");
    }
    
    
}