using DataAccess.Daos;
using DataAccess.Utils;
using Npgsql;
using Transversal.Utils;

namespace DataAccess.Repository;

public class GameRepository
{
    public UserScoreboardDao? GetScoreboardEntry(string username)
    {
        string selectQuery = "SELECT u.username, s.elo, s.wins, s.losses " +
                             "FROM scoreboard s " +
                             "JOIN users u ON s.user_id = u.id " +
                             "WHERE u.username = @username";
        
        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        using (NpgsqlCommand cmd = new NpgsqlCommand(selectQuery, conn))
        {
            try
            {
                conn.Open();

                cmd.Parameters.AddWithValue("username", username);

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new UserScoreboardDao()
                    {
                        Username = reader["username"].ToString(),
                        Elo = int.Parse(reader["elo"].ToString()),
                        Wins = int.Parse(reader["wins"].ToString()),
                        Losses = int.Parse(reader["losses"].ToString()),
                    };
                }

                conn.Close();
            }
            catch (Exception e)
            {
                Log.Error($"An error occured while adding cards", e);
            }
        }

        return new UserScoreboardDao();
    }
    
    public List<UserScoreboardDao> GetScoreboard()
    {
        string selectQuery = "SELECT u.username, s.elo, s.wins, s.losses " +
                             "FROM scoreboard s " +
                             "JOIN users u ON s.user_id = u.id " +
                             "ORDER BY s.elo DESC";
        
        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        using (NpgsqlCommand cmd = new NpgsqlCommand(selectQuery, conn))
        {
            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                List<UserScoreboardDao> scoreboard = new List<UserScoreboardDao>();

                while (reader.Read())
                {
                    scoreboard.Add(new UserScoreboardDao()
                    {
                        Username = reader["username"].ToString(),
                        Elo = int.Parse(reader["elo"].ToString()),
                        Wins = int.Parse(reader["wins"].ToString()),
                        Losses = int.Parse(reader["losses"].ToString()),
                    });
                }

                conn.Close();

                return scoreboard;
            }
            catch (Exception e)
            {
                Log.Error($"An error occured while adding cards", e);
            }
        }

        return new List<UserScoreboardDao>();
    }
    
    public void UpdateStats(UserDao player, bool Win)
    {
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE scoreboard SET wins = @wins, losses = @losses, elo = @elo WHERE user_id = @userId", conn))
            {
                conn.Open();
            
                cmd.Parameters.AddWithValue("@userId", player.Id);
                if (Win)
                {
                    cmd.Parameters.AddWithValue("@wins", player.Wins + 1);
                    cmd.Parameters.AddWithValue("@losses", player.Losses);
                    cmd.Parameters.AddWithValue("@elo", player.Elo + 3);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@wins", player.Wins );
                    cmd.Parameters.AddWithValue("@losses", player.Losses + 1);
                    cmd.Parameters.AddWithValue("@elo", player.Elo - 5);
                }
                cmd.ExecuteNonQuery();
            
                conn.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error($"An error occurred while updating stats for {player.Id}", e);;
            throw;
        }
    }
    
    public void SaveBattle(BattleDao battleDao)
    {
        string insertQuery = "INSERT INTO battles ( winner_id, opponent_id, log) VALUES ( @opponentId, @winnerId, @log)";

        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        using (NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, conn))
        {
            try
            {
                conn.Open();

                cmd.Parameters.AddWithValue("winnerId", battleDao.WinnerId);
                cmd.Parameters.AddWithValue("opponentId", battleDao.OpponentId);
                cmd.Parameters.AddWithValue("log", battleDao.Log);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception e)
            {
                Log.Error($"An error occured while adding cards", e);
            }
        }
    }

    public BattleDao FindLastBattle()
    {
        string selectQuery = "SELECT * FROM battles ORDER BY timestamp DESC LIMIT 1";

        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        using (NpgsqlCommand cmd = new NpgsqlCommand(selectQuery, conn))
        {
            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new BattleDao()
                    {
                        Id = Guid.Parse(reader["id"].ToString()),
                        WinnerId = Guid.Parse(reader["winner_id"].ToString()),
                        OpponentId = Guid.Parse(reader["opponent_id"].ToString()),
                        Log = reader["log"].ToString(),
                    };
                }

                conn.Close();
            }
            catch (Exception e)
            {
                Log.Error($"An error occured while adding cards", e);
            }
        }

        return new BattleDao();
    }
}