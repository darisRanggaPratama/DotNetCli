using MySql.Data.MySqlClient;

namespace MySqlCLIapp.Data
{
    public static class Database
    {
        private static readonly string _connectionString = new MySqlConnectionStringBuilder
        {
            Server = "localhost",
            Database = "db_sample",
            UserID = "rangga",
            Password = "rangga",
            Port = 3306,
            // SslMode setting removed for compatibility with current MySql.Data version
        }.ConnectionString;

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}