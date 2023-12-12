using Npgsql;
using DataAccess.Daos;
using DataAccess.Utils;

namespace DataAccess.Repository
{
    public class CardsRepository
    {
        public void CreateCards(List<CardsDao> cards)
        {
            if (cards == null || cards.Count == 0)
            {
                throw new ArgumentException("The list of cards cannot be null or empty.", nameof(cards));
            }

            string insertQuery = "INSERT INTO cards (id, name, damage) VALUES (@id, @name, @damage)";

            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, conn))
            {
                try
                {
                    conn.Open();

                    foreach (var card in cards)
                    {
                        Console.WriteLine(card.Name);
                        // Assuming Id is a Guid
                        cmd.Parameters.AddWithValue("@id", card.Id);
                        cmd.Parameters.AddWithValue("@name", card.Name);
                        cmd.Parameters.AddWithValue("@damage", card.Damage);

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

        public List<CardsDao> GetAllCards(Guid userId)
        {
            List<CardsDao> cardsList = new List<CardsDao>();

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
                            CardsDao card = GetCardById(cardId);
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
        
        public CardsDao? GetCardById(Guid cardId)
        {
            CardsDao card = null;
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
        
        private CardsDao MapCardFromDataReader(NpgsqlDataReader reader)
        {
            return new CardsDao()
            {
                Id = Guid.Parse(reader["id"].ToString()),
                Name = reader["name"].ToString(),
                Damage = Int16.Parse(reader["damage"].ToString())
            };
        }

        public List<CardsDao> GetDeck(Guid userId)
        {
            List<CardsDao> cardsList = new List<CardsDao>();

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM user_cards WHERE user_id = @userId AND is_in_deck = TRUE", conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Guid cardId = Guid.Parse(reader["card_id"].ToString());
                            CardsDao card = GetCardById(cardId);
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

        public bool ValidateDeckForConfiguration(Guid userId, List<CardsDao> cards, int deckSize)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
                {
                    conn.Open();

                    foreach (var card in cards)
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT(*) FROM user_cards WHERE user_id = @userId AND card_id = @cardId", conn))
                        {
                            cmd.Parameters.AddWithValue("@userId", userId);
                            cmd.Parameters.AddWithValue("@cardId", card.Id); // Assuming there is a property like CardId in CardsDao
                            int count = Convert.ToInt32(cmd.ExecuteScalar());

                            if (count == 0)
                            {
                                throw new ArgumentException($"Card with ID {card.Id} does not belong to the user or is not available.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking deck configuration: {ex.Message}");
                return false; // Indicate that an error occurred
            }

            if (deckSize != 4)
            {
                throw new ArgumentException("Deck size must be exactly 4.");
            }

            return true; // The deck configuration is valid
        }

        public void ConfigureDeck(Guid userId, List<CardsDao> cards)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
                {
                    conn.Open();

                    foreach (var card in cards)
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE user_cards SET is_in_deck = TRUE WHERE user_id = @userId AND card_id = @cardId", conn))
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@userId", userId);
                            cmd.Parameters.AddWithValue("@cardId", card.Id); 
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
                using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE user_cards SET is_in_deck = FALSE WHERE user_id = @userId", conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resetting deck for user {userId}: {ex.Message}");
            }
        }
    }
}