using DataAccess.Config;
using DataAccess.Daos;
using Npgsql;
    
namespace DataAccess.Repository
{
    public class UserRepository
    {
        
        public List<UserDao> GetAllUsers()
        {
            List<UserDao> users = new List<UserDao>();

            try
            {
                DatabaseManager.OpenDatabaseConnection();
                // Use the existing connection from the DatabaseManager
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM users", DatabaseManager.DbConnection))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserDao userDao = MapUserFromDataReader(reader);
                            users.Add(userDao);
                        }
                    }
                }
                DatabaseManager.CloseDatabaseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all users: {ex.Message}");
            }

            return users;
        }


        public UserDao? GetUserByUsername(string username)
        {
            try
            {
                DatabaseManager.OpenDatabaseConnection();
                // Use the existing connection from the DatabaseManager
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM users WHERE username = @username", DatabaseManager.DbConnection))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapUserFromDataReader(reader);
                        }
                    }
                }
                DatabaseManager.CloseDatabaseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user by username: {ex.Message}");
            }

            return null!; 
        }


        public void CreateUser(UserDao userDao)
        {
            DatabaseManager.OpenDatabaseConnection();
                // Use the existing connection from the DatabaseManager
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO users (username, password) VALUES (@username, @password)", DatabaseManager.DbConnection))
                {
                    cmd.Parameters.AddWithValue("@username", userDao.Username);
                    cmd.Parameters.AddWithValue("@password", userDao.Password);

                    cmd.ExecuteNonQuery();
                }
                DatabaseManager.CloseDatabaseConnection();
        }
        

        private UserDao MapUserFromDataReader(NpgsqlDataReader reader)
        {
            return new UserDao
            {
                Bio = reader["bio"].ToString(),
                Username = reader["username"].ToString(),
                Password = reader["password"].ToString(),
                Image = reader["image"].ToString(),
                // Add other properties as needed
            };
        }

        public UserDao UpdateUser(UserDao userDao)
        {
            try
            {
                DatabaseManager.OpenDatabaseConnection();
                // Use the existing connection from the DatabaseManager
                using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE users SET bio = @bio, image = @image WHERE username = @username RETURNING *", DatabaseManager.DbConnection))
                {
                    cmd.Parameters.AddWithValue("@username", userDao.Username);
                    cmd.Parameters.AddWithValue("@bio", userDao.Bio);
                    cmd.Parameters.AddWithValue("@image", userDao.Image);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapUserFromDataReader(reader);
                        }
                    }
                }
                DatabaseManager.CloseDatabaseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
            }

            return null; // Return null if the update fails or an error occurs
        }

        public bool UserExists(string username)
        {
            int count = 0;
            DatabaseManager.OpenDatabaseConnection();
            
            using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT COUNT(*) FROM users WHERE username = @username",
                       DatabaseManager.DbConnection))
            {
                cmd.Parameters.AddWithValue("@username", username);

                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            DatabaseManager.CloseDatabaseConnection();
            return count > 0;
        }
    }

}



