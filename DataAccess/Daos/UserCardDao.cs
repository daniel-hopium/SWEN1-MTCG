namespace DataAccess.Daos;

public class UserCardDao
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CardId { get; set; }
    public CardUsage Usage { get; set; }
}

/// <summary>
/// Is saved in all caps in DB
/// </summary>
public enum CardUsage
{
    None,
    Deck,
    Trade
}
