using Microsoft.Data.Sqlite;

namespace CrudCLIspectre.Services
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
			_connection.Open();
			var createTableQuery = @"CREATE TABLE IF NOT EXISTS Employees (
									Id INTEGER PRIMARY KEY AUTOINCREMENT,
									Name TEXT NOT NULL,
									Status INTEGER NOT NULL,
									Salary REAL NOT NULL
								);";
			using var command = _connection.CreateCommand();
			command.CommandText = createTableQuery;
			command.ExecuteNonQuery();
		}
	}
}
