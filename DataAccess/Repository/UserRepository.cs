using TradingCardGame.NET.persistence.entity;

namespace TradingCardGame.NET.persistence.repository;

using System;
using System.Collections.Generic;
using Npgsql;

public class UserRepository
{

    public List<User> GetAllUsers()
    {
        List<User> users = new List<User>();

        try
        {
            // Use the existing connection from the DatabaseManager
            using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM users", DatabaseManager.DbConnection))
            {
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = MapUserFromDataReader(reader);
                        users.Add(user);
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


    public User GetUserByUsername(string username)
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

        return null; 
    }


    public void AddUser(User user)
    {
        try
        {
            // Use the existing connection from the DatabaseManager
            using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO users (username, password) VALUES (@username, @password)", DatabaseManager.DbConnection))
            {
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@password", user.Password);

                cmd.ExecuteNonQuery();
            }
        }
        catch (PostgresException ex) when (ex.SqlState == "23505") // Unique violation
        {
            throw new UserAlreadyExistsException("Username already exists.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding user: {ex.Message}");
        }
    }
    
    // Add additional methods for update and delete as needed

    private User MapUserFromDataReader(NpgsqlDataReader reader)
    {
        return new User
        {
            Bio = reader["bio"].ToString(),
            Username = reader["username"].ToString(),
            Password = reader["password"].ToString(),
            Image = reader["image"].ToString(),
            // Add other properties as needed
        };
    }

    public User UpdateUser(User user)
    {
        try
        {
            // Use the existing connection from the DatabaseManager
            using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE users SET bio = @bio, image = @image WHERE username = @username RETURNING *", DatabaseManager.DbConnection))
            {
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@bio", user.Bio);
                cmd.Parameters.AddWithValue("@image", user.Image);

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
