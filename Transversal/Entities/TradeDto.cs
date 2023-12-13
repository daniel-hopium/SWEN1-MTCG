namespace Transversal.Entities;

public class TradeDto
{
    public Guid Id { get; set; }
    public Guid CardToTrade { get; set; }
    public string Type { get; set; }
    public int MinimumDamage { get; set; }
    
    public override string ToString()
    {
        return $"Id: {Id}, CardToTrade: {CardToTrade}, Type: {Type}, minimumDamage: {MinimumDamage}";
    }
}