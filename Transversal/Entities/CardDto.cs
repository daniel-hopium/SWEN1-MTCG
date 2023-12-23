namespace Transversal.Entities;

public class CardDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public float Damage { get; set; }
    public string? ElementType { get; set; }
    public string? CardType { get; set; }
    
}