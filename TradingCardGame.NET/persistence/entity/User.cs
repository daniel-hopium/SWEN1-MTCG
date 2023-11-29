namespace TradingCardGame.NET.persistence.entity;

public class User
{
    public string username { get; set; }
    public string password { get; set; }
    public string name { get; set; }
    public string bio { get; set; }
    public string image { get; set; }
    
    public override string ToString()
    {
        return $"Username: {username}, Name: {name}, Bio: {bio}, Image: {image}";
    }
    
}