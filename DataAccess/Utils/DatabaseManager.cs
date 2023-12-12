using System.Data;
using Npgsql;

namespace DataAccess.Utils
{
    public class DatabaseManager
    {
        private const string DefaultConnectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=postgres;Database=postgres";
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

        // Method to open the database connection
        public static void OpenDatabaseConnection()
        {
            if (_dbConnection == null)
            {
                _dbConnection = new NpgsqlConnection(ConnectionString);
                _dbConnection.Open();
            }
        }

        // Method to close the database connection
        public static void CloseDatabaseConnection()
        {
            if (_dbConnection is { State: ConnectionState.Open })
            {
                _dbConnection.Close();
                _dbConnection.Dispose();
            }
        }
    }
}