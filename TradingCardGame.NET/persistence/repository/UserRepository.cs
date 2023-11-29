using NpgsqlTypes;
using TradingCardGame.NET.persistence.entity;

namespace TradingCardGame.NET.persistence.repository;

using System;
using System.Collections.Generic;
using Npgsql;

public class UserRepository
{
    private readonly string _connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=postgres;Database=postgres";
    

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<User> GetAllUsers()
    {
        List<User> users = new List<User>();

        using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();

            string sql = "SELECT * FROM users";

            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
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

        return users;
    }

    public User GetUserById(int userId)
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();

            string sql = "SELECT * FROM users WHERE id = @userId";

            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.Add(new NpgsqlParameter("userId", NpgsqlDbType.Integer));
                // cmd.Parameters["userId"].Value = userId;

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapUserFromDataReader(reader);
                    }
                }
            }
        }

        return null; // User not found
    }

    public void AddUser(User user)
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
        {
            conn.Open();

            string sql = "INSERT INTO users (username, password) VALUES (@username, @password)";

            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("username", user.username);
                cmd.Parameters.AddWithValue("password", user.password);

                cmd.ExecuteNonQuery();
                Console.WriteLine("SUCCESSUFLLY CRAETED YOO!");
            }
        }
    }

    // Add additional methods for update and delete as needed

    private User MapUserFromDataReader(NpgsqlDataReader reader)
    {
        return new User
        {
            bio = reader["bio"].ToString(),
            username = reader["username"].ToString(),
            password = reader["password"].ToString(),
            // Add other properties as needed
        };
    }
}
