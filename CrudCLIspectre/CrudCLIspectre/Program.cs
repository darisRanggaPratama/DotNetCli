namespace CrudCLIspectre
{
	public class Program
	{
		static void Main(string[] args)
		{
			const string connectionString = "Data Source=employees.db";

			using var connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString);
			connection.Open();

			var databaseService = new Services.DatabaseService(connection);
			databaseService.InitializeDatabase();

			var employeeRepository = new Services.EmployeeRepository(connection);
			var employeeService = new Services.EmployeeService(employeeRepository);
			var menuUI = new UI.MenuUI(employeeService);

			menuUI.Run();
		}
	}
}