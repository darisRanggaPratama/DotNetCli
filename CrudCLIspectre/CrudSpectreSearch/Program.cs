using CrudSpectreSearch.Services;
using CrudSpectreSearch.UI;
using Microsoft.Data.Sqlite;

namespace CrudSpectreSearch
{

	public class Program
	{
		public static void Main(string[] args)
		{
			const string connectionString = "Data Source=employees.db";

			using var connection = new SqliteConnection(connectionString);
			connection.Open();

			var databaseService = new DatabaseService(connection);
			databaseService.InitializeDatabase();

			var employeeRepository = new EmployeeRepository(connection);
			var employeeService = new EmployeeService(employeeRepository);
			var menuUI = new MenuUI(employeeService);

			menuUI.Run();
		}
	}
}