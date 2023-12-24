using Npgsql;

namespace DataAccess.Utils;
public class DatabaseManager
{
    private const string DefaultConnectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=postgres;Database=trading_card_game_db";
    private static NpgsqlConnection? _dbConnection;

    // Public property to access the database connection string
    public static string ConnectionString { get; } = DefaultConnectionString;

    // Private constructor to enforce singleton pattern
    private DatabaseManager()
    {
    }

    // Property to access the database connection instance
    public static NpgsqlConnection DbConnection
    {
        get
        {
            if (_dbConnection == null)
            {
                throw new InvalidOperationException("Database connection has not been opened.");
            }

            return _dbConnection;
        }
    }

    //Method to run initial sql script
    public static void Initialize()
    {
        if (IsDatabaseSetup())
        {
            return;
        }
        try
        {
            _dbConnection = new NpgsqlConnection(ConnectionString);
            _dbConnection.Open();
            string filePath = "../../../../DataAccess/Initializer/sql/init.sql";
            var sql = System.IO.File.ReadAllText(filePath);
            var cmd = new NpgsqlCommand(sql, _dbConnection);
            cmd.ExecuteNonQuery();
            _dbConnection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        Console.WriteLine("Database setup complete");
    }
    
    //  Check if database is already setup
    private static bool IsDatabaseSetup()
    {
        try
        {
            _dbConnection = new NpgsqlConnection(ConnectionString);
            _dbConnection.Open();

            // Check the existence of necessary tables
            if (TableExists("users") && 
                TableExists("cards") && 
                TableExists("user_cards") && 
                TableExists("battles") && 
                TableExists("packages") && 
                TableExists("package_cards") && 
                TableExists("scoreboard"))
            {
                _dbConnection.Close();
                
                return true;
            }

            _dbConnection.Close();
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    private static bool TableExists(string tableName)
    {
        var sql = $"SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = '{tableName}')";

        using (var cmd = new NpgsqlCommand(sql, _dbConnection))
        {
            // ExecuteScalar is used to retrieve a single value (true or false)
            return (bool)cmd.ExecuteScalar();
        }
    }
    
}
