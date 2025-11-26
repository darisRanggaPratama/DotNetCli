using Microsoft.Data.Sqlite;

namespace CrudSpectreSearch.Services
{
	public class DatabaseService
	{
		private readonly SqliteConnection _connection;

		public DatabaseService(SqliteConnection connection)
		{
			_connection = connection;
		}

		public void InitializeDatabase()
		{
			var createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Employees (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Salary REAL NOT NULL,
                    Status INTEGER NOT NULL
                )";

			using var command = _connection.CreateCommand();
			command.CommandText = createTableQuery;
			command.ExecuteNonQuery();
		}
	}
}
