namespace DataAccess.Daos;

public class CardDao
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public double Damage { get; set; }
    public string ElementType { get; set; }
    public string CardType { get; set; }
    
    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Damage: {Damage}, ElementType: {ElementType}, CardType: {CardType}";
    }
}