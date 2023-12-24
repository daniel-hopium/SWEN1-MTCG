namespace DataAccess.Daos;

public class UserScoreboardDao
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public int Elo { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
}