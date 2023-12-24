namespace Transversal.Entities;

public class CardDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public double Damage { get; set; }
    public string? ElementType { get; set; }
    public string? CardType { get; set; }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}