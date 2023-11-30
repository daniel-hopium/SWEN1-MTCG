using DataAccess.Config;
using DataAccess.Daos;
using Npgsql;
    
namespace DataAccess.Repository
{
    public class UserRepository
    {
        public UserRepository()
        {
            DatabaseManager.OpenDatabaseConnection();
        }
        
        ~UserRepository()
        {
            DatabaseManager.CloseDatabaseConnection();
        }
        public List<UserDao> GetAllUsers()
        {
            List<UserDao> users = new List<UserDao>();

            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all users: {ex.Message}");
            }

            return users;
        }


        public UserDao GetUserByUsername(string username)
        {
            try
            {
                
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user by username: {ex.Message}");
            }

            return null!; 
        }


        public void AddUser(UserDao userDao)
        {
                // Use the existing connection from the DatabaseManager
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO users (username, password) VALUES (@username, @password)", DatabaseManager.DbConnection))
                {
                    cmd.Parameters.AddWithValue("@username", userDao.Username);
                    cmd.Parameters.AddWithValue("@password", userDao.Password);

                    cmd.ExecuteNonQuery();
                }
        }
        
        // Add additional methods for update and delete as needed

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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
            }

            return null; // Return null if the update fails or an error occurs
        }

    }

}



