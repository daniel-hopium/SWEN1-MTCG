using DataAccess.Daos;
using DataAccess.Utils;
using Npgsql;
    
namespace DataAccess.Repository;
public class UserRepository
{
    
    public UserDao? GetUserByUsername(string username)
    {
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM users WHERE username = @username", conn))
            {
                conn.Open();
                
                cmd.Parameters.AddWithValue("@username", username);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapUserFromDataReader(reader);
                    }
                }
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user by username {username}: {ex.Message}");
        }
        return null; 
    }


    public Guid CreateUser(UserDao userDao)
    {
        string insertQuery = "INSERT INTO users (username, password) VALUES (@username, @password) RETURNING id";

        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        using (NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, conn))
        {
            try
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@username", userDao.Username);
                cmd.Parameters.AddWithValue("@password", userDao.Password);

                // ExecuteScalar is used to retrieve the single value (in this case, the generated ID)
                Guid userId = (Guid)cmd.ExecuteScalar();

                conn.Close();

                return userId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user: {ex.Message}");
                return Guid.Empty; 
            }
        }
    }

    

    private UserDao MapUserFromDataReader(NpgsqlDataReader reader)
    {
        return new UserDao
        {
            Id = Guid.Parse(reader["id"].ToString()),
            Bio = reader["bio"].ToString(),
            Name = reader["name"].ToString(),
            Username = reader["username"].ToString(),
            Password = reader["password"].ToString(),
            Image = reader["image"].ToString(),
            Coins = Convert.ToInt32(reader["coins"]),
        };
    }

    public UserDao UpdateUser(UserDao userDao)
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE users SET username = @username, name = @name, bio = @bio, image = @image WHERE id = @id RETURNING id, username, name, bio, image, password, coins", conn))
        {
            conn.Open();
            
            cmd.Parameters.AddWithValue("@username", userDao.Username);
            cmd.Parameters.AddWithValue("@name", userDao.Name);
            cmd.Parameters.AddWithValue("@bio", userDao.Bio);
            cmd.Parameters.AddWithValue("@image", userDao.Image);
            cmd.Parameters.AddWithValue("@id", userDao.Id);

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return MapUserFromDataReader(reader);
                }
            }
            
            conn.Close();
        }

        return null;
    }

    public bool UserExists(string username)
    {
        int count = 0;
        
        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT COUNT(*) FROM users WHERE username = @username",
                   conn))
        {
            conn.Open();
            
            cmd.Parameters.AddWithValue("@username", username);
            count = Convert.ToInt32(cmd.ExecuteScalar());
            
            conn.Close();
        }
        
        return count > 0;
    }

    public void UpdateUserMoney(UserDao user, int packagePrice)
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE users SET coins = @coins WHERE username = @username", conn))
        {
            conn.Open();
            
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@coins", user.Coins - packagePrice);
            Console.WriteLine(packagePrice);
            cmd.ExecuteNonQuery();
            
            conn.Close();
        }
    }

    public void UpdateStats(UserDao player, bool Win)
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE users SET wins = @wins, losses = @losses WHERE username = @username", conn))
        {
            conn.Open();
            
            cmd.Parameters.AddWithValue("@username", player.Username);
            if (Win)
            {
                cmd.Parameters.AddWithValue("@wins", player.Wins + 1);
                cmd.Parameters.AddWithValue("@losses", player.Losses);
            }
            else
            {
                cmd.Parameters.AddWithValue("@wins", player.Wins );
                cmd.Parameters.AddWithValue("@losses", player.Losses + 1);
            }
            cmd.ExecuteNonQuery();
            
            conn.Close();
        }
    }

    public List<UserDao> GetScoreboard()
    {
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT username, wins, losses FROM users ORDER BY wins DESC", conn))
            {
                conn.Open();
                
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    List<UserDao> users = new List<UserDao>();
                    while (reader.Read())
                    {
                        users.Add(new UserDao
                        {
                            Username = reader["username"].ToString(),
                            Wins = Convert.ToInt32(reader["wins"]),
                            Losses = Convert.ToInt32(reader["losses"])
                        });
                    }
                    return users;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public UserDao GetUserWithScoreByUsername(string username)
    {
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM users u " +
                                                         "JOIN scoreboard s ON s.user_id = u.id " +
                                                         "WHERE u.username = @username", conn))
            {
                conn.Open();
            
                cmd.Parameters.AddWithValue("@username", username);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var user = MapUserFromDataReader(reader);
                        user.Wins = Convert.ToInt32(reader["wins"]);
                        user.Losses = Convert.ToInt32(reader["losses"]);
                        user.Elo = Convert.ToInt32(reader["elo"]);
                        return user;
                    }
                }
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user by username {username}: {ex.Message}");
        }
        return null; 
    }

    public void CreateUserScore(Guid userId)
    {
        string insertQuery = "INSERT INTO scoreboard (user_id) VALUES (@user_id)";

        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        using (NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, conn))
        {
            try
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@user_id", userId);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user: {ex.Message}");
            }
        }
    }
}




