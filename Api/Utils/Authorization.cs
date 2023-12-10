namespace Api.Utils;

public static class Authorization
{
    private static string Type { get;  set; } 
    private static string Token { get;  set; }
    
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
}