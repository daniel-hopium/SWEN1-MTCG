using System.Threading.Channels;
using Npgsql;
using DataAccess.Daos;
using DataAccess.Utils;
using static DataAccess.Repository.CardsRepository.Usage;

namespace DataAccess.Repository;

public class CardsRepository
{
    public enum Usage
    {
        Deck,
        Trade,
        None
    }
    
    public void CreateCards(List<CardDao> cards)
{
    if (cards == null || cards.Count == 0)
    {
        throw new ArgumentException("The list of cards cannot be null or empty.", nameof(cards));
    }

    string insertQuery = "INSERT INTO cards (id, name, damage, element_type, card_type) VALUES (@id, @name, @damage, @elementType, @cardType)";

    using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
    {
        conn.Open();

        using (NpgsqlTransaction transaction = conn.BeginTransaction())
        {
            try
            {
                foreach (var card in cards)
                {
                    // Check if the card already exists
                    if (CardExists(conn, transaction, card.Id))
                    {
                        Console.WriteLine($"Card with ID '{card.Id}' already exists.");
                        throw new InvalidOperationException("At least one card in the packages already exists");
                    }

                    using (NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id", card.Id);
                        cmd.Parameters.AddWithValue("@name", card.Name);
                        cmd.Parameters.AddWithValue("@damage", card.Damage);
                        cmd.Parameters.AddWithValue("@elementType", card.ElementType != null ? card.ElementType : DBNull.Value);
                        cmd.Parameters.AddWithValue("@cardType", card.ElementType != null ? card.CardType : DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error adding cards: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }
    }
}

    private bool CardExists(NpgsqlConnection conn, NpgsqlTransaction transaction, Guid cardId)
{
    string checkQuery = "SELECT COUNT(*) FROM cards WHERE id = @id";
    using (NpgsqlCommand cmd = new NpgsqlCommand(checkQuery, conn, transaction))
    {
        cmd.Parameters.AddWithValue("@id", cardId);
        int count = Convert.ToInt32(cmd.ExecuteScalar());
        return count > 0;
    }
}
    
    public List<CardDao> GetAllCards(Guid userId)
    {
        List<CardDao> cardsList = new List<CardDao>();

        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM user_cards WHERE user_id = @userId", conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@userId", userId);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Guid cardId = Guid.Parse(reader["card_id"].ToString());
                        CardDao card = GetCardById(cardId);
                        if (card != null)
                        {
                            cardsList.Add(card);
                        }
                    }
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching cards for userId {userId}: {ex.Message}");
        }

        return cardsList;
    }
    
    public CardDao? GetCardById(Guid cardId)
    {
        CardDao card = null;
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM cards WHERE id = @cardId", conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@cardId", cardId);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        card = MapCardFromDataReader(reader);
                    }
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching card by ID {cardId}: {ex.Message}");
        }

        return card;
    }
    
    private CardDao MapCardFromDataReader(NpgsqlDataReader reader)
    {
        return new CardDao()
        {
            Id = Guid.Parse(reader["id"].ToString()),
            Name = reader["name"].ToString(),
            Damage = Int16.Parse(reader["damage"].ToString()),
            ElementType = reader["element_type"].ToString(),
            CardType = reader["card_type"].ToString()
        };
    }

    public List<CardDao> GetDeck(Guid userId)
    {
        List<CardDao> cardsList = new List<CardDao>();

        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM user_cards WHERE user_id = @userId AND usage = @usage", conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@usage", Deck.ToString().ToLower());

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Guid cardId = Guid.Parse(reader["card_id"].ToString());
                        CardDao card = GetCardById(cardId);
                        if (card != null)
                        {
                            cardsList.Add(card);
                        }
                    }
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching cards for userId {userId}: {ex.Message}");
        }
        return cardsList;
    }

    public bool ValidateDeckForConfiguration(Guid userId, List<CardDao> cards, int deckSize)
    {
        var validCards = 0;
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            {
                conn.Open();
        
                foreach (var card in cards)
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT(*) FROM user_cards WHERE user_id = @userId AND card_id = @cardId AND usage = @usage", conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@cardId", card.Id);
                        cmd.Parameters.AddWithValue("@usage", None.ToString().ToLower());
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count == 0)
                        {
                            throw new ArgumentException($"Card with ID {card.Id} does not belong to the user {userId} or is not available.");
                        }
                        validCards++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking deck configuration: {ex.Message}");
            return false; // Indicate that an error occurred
        }

        if (validCards == deckSize) return true; // The deck configuration is valid
        Console.WriteLine($"Error checking deck configuration: Deck size must be exactly {deckSize}.");
        return false;

    }

    public void ConfigureDeck(Guid userId, List<CardDao> cards)
    {
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            {
                conn.Open();

                foreach (var card in cards)
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE user_cards SET usage = @usage WHERE user_id = @userId AND card_id = @cardId", conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@cardId", card.Id); 
                        cmd.Parameters.AddWithValue("@usage", Deck.ToString().ToLower());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error configuring Deck: {ex.Message}");
        }
    }

    public void ResetDeck(Guid userId)
    {
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE user_cards SET usage = @newUsage WHERE user_id = @userId AND usage = @oldUsage", conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@newUsage", None.ToString().ToLower());
                cmd.Parameters.AddWithValue("@oldUsage", Deck.ToString().ToLower());
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error resetting deck for user {userId}: {ex.Message}");
        }
    }

    public UserCardDao? GetUserCardByUserAndCardId(Guid userId, Guid cardId, Usage usage)
    {
        UserCardDao userCard = null;
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM user_cards WHERE card_id = @cardId AND user_id = @userId AND usage = @usage", conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@cardId", cardId);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@usage", usage.ToString().ToLower());

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserCardDao()
                        {
                            Id = Guid.Parse(reader["id"].ToString()),
                            UserId = userId,
                            CardId = cardId
                        };
                    }
                }

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching card by ID {cardId}: {ex.Message}");
        }

        return null;
    }
    
    public UserCardDao GetUserCardByCardId(Guid cardId)
    {
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM user_cards WHERE card_id = @cardId", conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@cardId", cardId);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapUserCardFromDataReader(reader);
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching card by ID {cardId}: {ex.Message}");
            }
        }
        return null;
    }
    
    private UserCardDao MapUserCardFromDataReader(NpgsqlDataReader reader)
    {
        return new UserCardDao()
        {
            Id = Guid.Parse(reader["id"].ToString()),
            UserId = Guid.Parse(reader["user_id"].ToString()),
            CardId = Guid.Parse(reader["card_id"].ToString()),
            Usage = (CardUsage)Enum.Parse(typeof(Usage), reader["usage"].ToString(), true)
        };
    }
    
    public void UpdateUsage(Guid userId, Guid userCardCardId, Usage trade)
    {
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE user_cards SET usage = @usage WHERE user_id = @userId AND card_id = @cardId", conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@cardId", userCardCardId);
                cmd.Parameters.AddWithValue("@usage", trade.ToString().ToLower());
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating usage for user {userId} and card {userCardCardId}: {ex.Message}");
        }
    }
    
}