
namespace DataAccess.Daos
{
    
    public class UserDao
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public string? Image { get; set; }
        public int Coins { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
    
    }    
}

