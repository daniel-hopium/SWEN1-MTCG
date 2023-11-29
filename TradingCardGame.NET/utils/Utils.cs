namespace TradingCardGame.NET.utils;

public class Utils
{
    public string PathVariable(string path)
    {
        string[] pathSegments = path.Split("/");
        if (pathSegments.Length < 2 || pathSegments.Length > 4)
        {
            throw new Exception("Invalid Path Variable Format");
        }
        return  pathSegments[2]; 
    }
}