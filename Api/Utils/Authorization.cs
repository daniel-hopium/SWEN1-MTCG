using API.HttpServer;
using BusinessLogic.Services;
using Transversal.Entities;

namespace Api.Utils;

public static class Authorization
{
    private static string? Type { get;  set; } 
    private static string? Token { get;  set; }
    
    private static readonly UserService UserService = new UserService();
    
    public static bool AuthorizeAdmin(string authorization)
    {
        if(string.IsNullOrWhiteSpace(authorization)) return false;
        
        string[] parts = authorization.Split(' ');
        if(parts.Length != 2) return false;
        
        Type = parts[0];
        Token = parts[1];
        
        if (Type != "Bearer") return false;
        if (Token != "admin-mtcgToken") return false;

        return true;
    }

    public static bool AuthorizeUser(string authorization)
    {
        if(string.IsNullOrWhiteSpace(authorization)) return false;
        
        string[] parts = authorization.Split(' ');
        if (parts.Length != 2) return false;
        
        Type = parts[0];
        Token = parts[1];
        var tokenParts = Token.Split('-');
        if (tokenParts.Length != 2) return false;
        if (Type != "Bearer") return false;
        if (tokenParts[1] != "mtcgToken") return false;
        
        return UserService.UserExists(tokenParts[0]);
    }

    public static string GetUsernameFromAuthorization(string authorization)
    {
        string[] parts = authorization.Split(' ');
        Token = parts[1];
        
        var tokenParts = Token.Split('-');
        return tokenParts[0];
    }
    
    public static bool UserIsAuthorized (HttpSvrEventArgs e)
    {
        if (AuthorizeUser(e.Authorization)) return true;
        
        e.Reply(401, "Unauthorized");
        return false;
    }
}