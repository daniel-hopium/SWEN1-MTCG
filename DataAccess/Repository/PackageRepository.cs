using DataAccess.Daos;
using DataAccess.Utils;
using Npgsql;

namespace DataAccess.Repository;

public class PackageRepository
{
    public void CreatePackage(List<CardDao> cards, Guid packageId)
    {
        if (cards == null || cards.Count == 0)
        {
            throw new ArgumentException("The list of cards cannot be null or empty.", nameof(cards));
        }
        
        string insertQuery = "INSERT INTO packages (id) VALUES (@packageId)";

        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        using (NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, conn))
        {
            try
            {
                conn.Open();
                
                cmd.Parameters.AddWithValue("@packageId", packageId);
                cmd.ExecuteNonQuery();
                    
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding cards: {ex.Message}");
            }
        }
        
        AddCardsToPackage(cards, packageId);
        
    }

    public void AddCardsToPackage(List<CardDao> cards, Guid packageId)
    {
        string insertQuery = "INSERT INTO package_cards (package_id, card_id) VALUES (@packageId, @cardId)";

        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        using (NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, conn))
        {
            try
            {
                conn.Open();

                foreach (var card in cards)
                {
                    cmd.Parameters.AddWithValue("@packageId", packageId);
                    cmd.Parameters.AddWithValue("@cardId", card.Id);

                    cmd.ExecuteNonQuery();

                    // Clear parameters for the next iteration
                    cmd.Parameters.Clear();
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding cards: {ex.Message}");
            }
        }
    }

    public PackageDao FindPackage(Guid packageId)
    {
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            {
                conn.Open();

                using (NpgsqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        PackageDao package = FetchPackage(packageId, conn, transaction);
                        
                        // Load associated cards
                        package.Cards = LoadCardsForPackage(packageId, conn, transaction);

                        transaction.Commit();
                        return package;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in transaction: {ex.Message}");
                        transaction.Rollback();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching package by id: {ex.Message}");
        }
        return null!;
    }

private PackageDao FetchPackage(Guid packageId, NpgsqlConnection connection, NpgsqlTransaction transaction)
{
    using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM packages WHERE id = @packageId", connection, transaction))
    {
        cmd.Parameters.AddWithValue("@packageId", packageId);

        using (NpgsqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                return MapPackageFromDataReader(reader);
            }
        }
    }

    return null;
}

private List<CardDao> LoadCardsForPackage(Guid packageId, NpgsqlConnection connection, NpgsqlTransaction transaction)
{
    List<CardDao> cards = new List<CardDao>();

    using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT card_id FROM package_cards WHERE package_id = @packageId", connection, transaction))
    {
        cmd.Parameters.AddWithValue("@packageId", packageId);

        using (NpgsqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                Guid cardId = reader.GetGuid(reader.GetOrdinal("card_id"));
                
                if (cardId != null)
                {
                    cards.Add(new CardDao()
                    {
                        Id = cardId
                    });
                }
            }
        }
    }

    return cards;
}

    private PackageDao MapPackageFromDataReader(NpgsqlDataReader reader)
    {
        return new PackageDao()
        {
            Id = Guid.Parse(reader["id"].ToString()),
            Name = reader["name"].ToString(),
            Price = Int16.Parse(reader["price"].ToString())
        };
    }

    public void AddPackageToUser(UserDao user, PackageDao daoPackage)
    {
        string insertQuery = "INSERT INTO user_cards (user_id, card_id) VALUES (@userId, @cardId)";

        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        {
            conn.Open();

            foreach (var card in daoPackage.Cards)
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, conn))
                {
                    try
                    {
                        cmd.Parameters.Clear(); // Clear previous parameters

                        cmd.Parameters.AddWithValue("@userId", user.Id);
                        cmd.Parameters.AddWithValue("@cardId", card.Id);

                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding card {card.Id} to user {user.Id}: {ex.Message}");
                    }
                }
            }

            conn.Close();
        }
    }

    public PackageDao FindLastestPackage()
    {
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            {
                conn.Open();

                using (NpgsqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        PackageDao package = FetchLatestPackage(conn, transaction);
                        
                        // Load associated cards
                        package.Cards = LoadCardsForPackage(package.Id, conn, transaction);

                        transaction.Commit();
                        return package;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in transaction: {ex.Message}");
                        transaction.Rollback();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching package by id: {ex.Message}");
        }
        return null!;
    }
    
    private PackageDao FetchLatestPackage(NpgsqlConnection conn, NpgsqlTransaction transaction)
    {
        string query = "SELECT * FROM packages ORDER BY created DESC LIMIT 1";
        using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn, transaction))
        {
            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return MapPackageFromDataReader(reader);
                }
            }
        }

        return null;
    }

    public void DeletePackage(PackageDao daoPackage)
    {
        string deleteQuery = "DELETE FROM packages WHERE id = @packageId";

        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        {
            conn.Open();

            using (NpgsqlCommand cmd = new NpgsqlCommand(deleteQuery, conn))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@packageId", daoPackage.Id);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting package {daoPackage.Id}: {ex.Message}");
                }
            }

            conn.Close();
        }
    }
}