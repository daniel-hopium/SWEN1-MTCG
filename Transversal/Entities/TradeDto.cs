namespace Transversal.Entities;

public class TradeDto
{
    public Guid Id { get; set; }
    public Guid CardToTrade { get; set; }
    public string Type { get; set; }
    public int Damage { get; set; }
    
}