namespace TradingCardGame.NET.persistence.entity;

public class User
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Name { get; set; }
    public string? Bio { get; set; }
    public string? Image { get; set; }
    
    public override string ToString()
    {
        return $"Username: {Username}, Name: {Name}, Bio: {Bio}, Image: {Image}";
    }
    
}