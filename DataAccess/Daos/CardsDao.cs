namespace DataAccess.Daos;

public class CardsDao
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public float Damage { get; set; }
    
    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Damage: {Damage}";
    }
}