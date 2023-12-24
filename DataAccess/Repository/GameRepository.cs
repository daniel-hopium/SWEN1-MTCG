using DataAccess.Daos;
using DataAccess.Utils;
using Npgsql;

namespace DataAccess.Repository;

public class GameRepository
{
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding cards: {ex.Message}");
            }
        }
    }

    public BattleDao FindLastBattle()
    {
        string selectQuery = "SELECT * FROM battles ORDER BY id DESC LIMIT 1";

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
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding cards: {ex.Message}");
            }
        }

        return new BattleDao();
    }
}