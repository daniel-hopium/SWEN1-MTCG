namespace Transversal.Entities;

public class TradeDto
{
    public Guid Id { get; set; }
    public Guid CardId { get; set; }
    public string Type { get; set; }
    public int MinimumDamage { get; set; }
    
    public override string ToString()
    {
        return $"Id: {Id}, CardToTrade: {CardId}, Type: {Type}, minimumDamage: {MinimumDamage}";
    }
}