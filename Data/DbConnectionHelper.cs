using MySql.Data.MySqlClient;

namespace MalshinonApp.Data
{
    public static class DbConnectionHelper
    {
        private static readonly string connectionString =
            "server=localhost;user=root;password=;database=malshinon";

        public static MySqlConnection GetConnection()
        {
            var connection = new MySqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}