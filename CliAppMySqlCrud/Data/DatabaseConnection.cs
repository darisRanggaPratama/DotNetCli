using MySql.Data.MySqlClient;

namespace CliAppMySqlCrud.Data;

public class DatabaseConnection
{
    private readonly string _connectionString;

    public DatabaseConnection()
    {
        _connectionString = "Server=localhost;Database=db_sample;Uid=rangga;Pwd=rangga;";
    }

    public MySqlConnection GetConnection()
    {
        return new MySqlConnection(_connectionString);
    }

    public void InitializeDatabase()
    {
        using var connection = GetConnection();
        connection.Open();

        string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS employee (
                    row_id INT AUTO_INCREMENT PRIMARY KEY,
                    id VARCHAR(6) NOT NULL UNIQUE,
                    name VARCHAR(255) NOT NULL,
                    salary DECIMAL(15,2) NOT NULL,
                    status TINYINT(1) NOT NULL DEFAULT 0
                )";

        using var command = new MySqlCommand(createTableQuery, connection);
        command.ExecuteNonQuery();
    }
}