namespace DataAccess.Daos;

public class TradeDao
{
    public Guid Id { get; set; }
    public Guid CardToTrade { get; set; }
    public string Type { get; set; }
    public int MinimumDamage { get; set; }
    
}