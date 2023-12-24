
namespace DataAccess.Daos;

public class BattleDao
{
    public Guid Id { get; set; }
    public Guid WinnerId { get; set; }
    public Guid OpponentId { get; set; }
    public String Log { get; set; }
    public string Timestamp { get; set; }
}