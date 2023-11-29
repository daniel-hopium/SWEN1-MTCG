using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using TradingCardGame.NET.persistence.entity;
using TradingCardGame.NET.service;
using TradingCardGame.NET.utils;

namespace TradingCardGame.NET.Controller;

public class UserController
{
    private UserService _userService = new UserService();
    private Utils _utils = new Utils();
    
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
        else if (e.Path.Equals("/sessions") && e.Method.Equals("POST"))
        {
            Login(e);
        }
    }

    private void Login(HttpSvrEventArgs httpSvrEventArgs)
    {
        throw new NotImplementedException();
    }

    private void UpdateUserData(HttpSvrEventArgs e)
    {
        User user = new User();

        string pathVariable = _utils.PathVariable(e.Path);
        Console.WriteLine("PATHSEGMENT " + pathVariable);
        
        JsonDocument jsonDocument = JsonDocument.Parse(e.Payload);
        JsonElement root = jsonDocument.RootElement;
        
        user.username = pathVariable;
        user.name = root.GetProperty("name").GetString(); // FIX NULLABLE
        user.bio = root.GetProperty("bio").GetString();
        user.image = root.GetProperty("image").GetString();

        _userService.UpdateUser(user);
        
        
        e.Reply(200, "Successfully updated data");
    }

    private void GetUserData(HttpSvrEventArgs e)
    {
        string username = _utils.PathVariable(e.Path);
        
        JsonDocument jsonDocument = JsonDocument.Parse(e.Payload);
        JsonElement root = jsonDocument.RootElement;

        _userService.GetData(username);
        
        e.Reply(200, "DATA");
    }

    private void CreateUser(HttpSvrEventArgs e)
    {
        JsonDocument jsonDocument = JsonDocument.Parse(e.Payload);
        JsonElement root = jsonDocument.RootElement;
        
        User user = new User();
        
        Console.WriteLine(e.Payload);
        user.username = root.GetProperty("username").GetString();
        user.password = root.GetProperty("password").GetString();
        _userService.CreateUser(user);
        e.Reply(201, "User successfully created");
    }
    
    
}