﻿using DataAccess.Config;
using DataAccess.Daos;
using Npgsql;
    
namespace DataAccess.Repository
{
    public class UserRepository
    {
        
        public UserDao? GetUserByUsername(string username)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM users WHERE username = @username", conn))
                {
                    conn.Open();
                    
                    cmd.Parameters.AddWithValue("@username", username);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapUserFromDataReader(reader);
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user by username: {ex.Message}");
            }
            return null!; 
        }


        public void CreateUser(UserDao userDao)
        {
            string insertQuery = "INSERT INTO users (username, password) VALUES (@username, @password)";

            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand(insertQuery, conn))
            {
                try
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@username", userDao.Username);
                    cmd.Parameters.AddWithValue("@password", userDao.Password);

                    cmd.ExecuteNonQuery();
                    
                    conn.Close();
                    conn.Dispose();
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating user: {ex.Message}");
                }
            }
            
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
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE users SET bio = @bio, image = @image WHERE username = @username RETURNING *", DatabaseManager.DbConnection))
            {
                conn.Open();
                
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
                
                conn.Close();
                conn.Dispose();
            }

            return null;
        }

        public bool UserExists(string username)
        {
            int count = 0;
            
            using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
            using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT COUNT(*) FROM users WHERE username = @username",
                       conn))
            {
                conn.Open();
                
                cmd.Parameters.AddWithValue("@username", username);
                count = Convert.ToInt32(cmd.ExecuteScalar());
                
                conn.Close();
                conn.Dispose();
            }
            
            return count > 0;
        }
    }

}



