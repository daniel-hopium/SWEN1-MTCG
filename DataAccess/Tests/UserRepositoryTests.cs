using DataAccess.Daos;
using DataAccess.Repository;
using DataAccess.Utils;
using Npgsql;
using NUnit.Framework;

namespace DataAccess.Tests;

[TestFixture]
public class UserRepositoryTests
{

    [SetUp]
    public void Setup()
    {
        
        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM users;", conn))
        {
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    [Test(Description = "Test fetching a user by a valid username.")]
    public void GetUserByUsername_ValidUsername_ReturnsUserDao()
    {
        // Arrange
        var userRepository = new UserRepository();
        var expectedUser = new UserDao
        {
            Username = "testuser",
            Password = "testpassword",
            // Set other properties as needed for a valid user
        };
        userRepository.CreateUser(expectedUser);

        // Act
        var actualUser = userRepository.GetUserByUsername("testuser");

        // Assert
        Assert.That(actualUser, Is.Not.Null);
        Assert.That(actualUser.Username, Is.EqualTo(expectedUser.Username));
    }

    [Test(Description = "Test creating a new user and retrieving it.")]
    public void CreateUser_NewUser_SuccessfullyCreatesUser()
    {
        // Arrange
        var userRepository = new UserRepository();
        var newUser = new UserDao
        {
            Username = "newuser",
            Password = "newpassword",
        };

        // Act
        userRepository.CreateUser(newUser);
        var retrievedUser = userRepository.GetUserByUsername("newuser");

        // Assert
        Assert.That(retrievedUser, Is.Not.Null);
        Assert.That(newUser.Username, Is.EqualTo(retrievedUser.Username));
    }

    // Similar tests for UpdateUser, UserExists, etc.

    [TearDown]
    public void TearDown()
    {
        using (NpgsqlConnection conn = new NpgsqlConnection(DatabaseManager.ConnectionString))
        using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM users;", conn))
        {
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}

