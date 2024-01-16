using DataAccess.Daos;
using DataAccess.Utils;
using Npgsql;
using Transversal.Utils;
using static DataAccess.Repository.CardsRepository.Usage;

namespace DataAccess.Repository;

public class TradeRepository
{
    public List<TradeDao> GetTrades()
    {
        List<TradeDao> tradingsList = new List<TradeDao>();

        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM trades JOIN user_cards ON trades.card_id = user_cards.card_id", conn))
            {
                conn.Open();

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tradingsList.Add(MapTradeFromDataReader(reader));
                    }
                }

                conn.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error($"An error occured while getting trades", e);
        }
        return tradingsList;
    }

    private TradeDao MapTradeFromDataReader(NpgsqlDataReader reader)
    {
        return new TradeDao()
        {
            Id = Guid.Parse(reader["id"].ToString()),
            CardToTradeId = Guid.Parse(reader["card_id"].ToString()),
            Type = reader["type"].ToString(),
            MinimumDamage = int.Parse(reader["minimum_damage"].ToString())
        };
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
        catch (Exception e)
        {
            Log.Error($"An error occured while checking if a trade exists", e);
        }
        return false;
    }

    public void CreateTrade(TradeDao tradeDao)
    {
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO trades (id, card_id, type, minimum_damage) VALUES (@id, @cardToTrade, @type, @minimumDamage)", conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("id", tradeDao.Id);
                cmd.Parameters.AddWithValue("cardToTrade", tradeDao.CardToTradeId);
                cmd.Parameters.AddWithValue("type", tradeDao.Type);
                cmd.Parameters.AddWithValue("minimumDamage", tradeDao.MinimumDamage);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error($"An error occured while creating a trade", e);
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
        catch (Exception e)
        {
            Log.Error($"An error occured while deleting trade {tradeId}", e);
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
                        return MapTradeFromDataReader(reader);
                    }
                }

                conn.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error($"An error occured while getting trades", e);
        }
        return null;
    }

    public void UpdateCards(Guid newUserId, Guid cardId)
    {
        string updateQuery = "UPDATE user_cards SET usage = @usage, user_id = @newUserId WHERE card_id = @card_id";

        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        using (NpgsqlCommand cmd = new NpgsqlCommand(updateQuery, conn))
        {
            try
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@user_id", newUserId);
                cmd.Parameters.AddWithValue("@card_id", cardId);
                cmd.Parameters.AddWithValue("@usage", None.ToString());

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception e)
            {
                Log.Error($"An error occured while updating a card", e);
            }
        }
    }
}