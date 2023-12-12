using DataAccess.Daos;
using DataAccess.Repository;
using DataAccess.Utils;
using Npgsql;
using NUnit.Framework;

namespace DataAccess.Tests;

[TestFixture]
public class PackageRepositoryTest
{

    [SetUp]
    public void Setup()
    {
        
        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM cards; DELETE FROM packages; DELETE FROM packages;", conn))
        {
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    [Test(Description = "Test fetching a user by a valid username.")]
    public void GetUserByUsername_ValidUsername_ReturnsUserDao()
    {
        // Arrange
        Console.WriteLine("LOLWUT");
    }

    [TearDown]
    public void TearDown()
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM cards; DELETE FROM packages; DELETE FROM packages;", conn))
        {
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}

