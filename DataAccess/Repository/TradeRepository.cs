using DataAccess.Daos;
using DataAccess.Utils;
using Npgsql;

namespace DataAccess.Repository;

public class TradeRepository
{
    public List<TradeDao> GetTrades()
    {
        List<TradeDao> tradingsList = new List<TradeDao>();

        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM trades", conn))
            {
                conn.Open();

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tradingsList.Add(
                            new TradeDao()
                        {
                            Id = Guid.Parse(reader["id"].ToString()),
                            UserCardToTradeId = Guid.Parse(reader["card_to_trade"].ToString()),
                            Type = reader["type"].ToString(),
                            MinimumDamage = int.Parse(reader["damage"].ToString())
                        });
                    }
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting tradings: {ex.Message}");
        }
        return tradingsList;
    }

    public bool Exists(Guid tradeId)
    {
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM trades WHERE id = @id", conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("id", tradeId);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting tradings: {ex.Message}");
        }
        return false;
    }

    public void CreateTrade(TradeDao tradeDao)
    {
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO trades (id, user_card_id, minimum_damage) VALUES (@id, @card_to_trade, @minimumDamage)", conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("id", tradeDao.Id);
                cmd.Parameters.AddWithValue("card_to_trade", tradeDao.UserCardToTradeId);
                cmd.Parameters.AddWithValue("damage", tradeDao.MinimumDamage);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating trade: {ex.Message}");
        }
    }

    public void DeleteTrade(Guid tradeId)
    {
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM trades WHERE id = @id", conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("id", tradeId);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting trade: {ex.Message}");
        }
    }

    public TradeDao? GetTrade(Guid tradeId)
    {
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM trades WHERE id = @id", conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("id", tradeId);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TradeDao()
                        {
                            Id = Guid.Parse(reader["id"].ToString()),
                            UserCardToTradeId = Guid.Parse(reader["card_to_trade"].ToString()),
                            Type = reader["type"].ToString(),
                            MinimumDamage = int.Parse(reader["damage"].ToString())
                        };
                    }
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting tradings: {ex.Message}");
        }
        return null;
    }
}