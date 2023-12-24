namespace Transversal.Entities;

public class UserScoreboardDto
{
    public string Username { get; set; }
    public int Elo { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
}