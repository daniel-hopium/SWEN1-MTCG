using DataAccess.Daos;
using DataAccess.Utils;
using Npgsql;

namespace DataAccess.Repository;

public class TradeRepository
{
    public List<TradeDao> GetTradings()
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
                            CardToTrade = Guid.Parse(reader["card_to_trade"].ToString()),
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
}