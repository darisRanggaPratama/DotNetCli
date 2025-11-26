using CrudCLIspectre.Models;
using Microsoft.Data.Sqlite;

namespace CrudCLIspectre.Services
{
	public class EmployeeRepository : IEmployeeRepository
	{
		private readonly SqliteConnection _connection;

		public EmployeeRepository(SqliteConnection connection)
		{
			_connection = connection;
		}

		public void Add(Employee employee)
		{
			var query = "INSERT INTO Employees (Name, status, Salary) VALUES (@Name, @status, @Salary);";
			using var command = _connection.CreateCommand();
			command.CommandText = query;
			command.Parameters.AddWithValue("@Name", employee.Name);
			command.Parameters.AddWithValue("@status", employee.Status ? 1 : 0);
			command.Parameters.AddWithValue("@Salary", employee.Salary);
			command.ExecuteNonQuery();
		}

		public IEnumerable<Employee> GetAll()
		{
			var employees = new List<Employee>();
			var query = "SELECT Id, Name, Status, Salary FROM Employees ORDER BY Id;";
			using var command = _connection.CreateCommand();
			command.CommandText = query;
			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				employees.Add(new Employee
				{
					Id = reader.GetInt32(0),
					Name = reader.GetString(1),
					Status = reader.GetInt32(2) == 1,
					Salary = reader.GetDecimal(3)
				});
			}
			return employees;
		}

		public Employee? GetById(int id)
		{
			var query = "SELECT Id, Name, Status, Salary FROM Employees WHERE Id = @Id;";
			using var command = _connection.CreateCommand();
			command.CommandText = query;
			command.Parameters.AddWithValue("@Id", id);
			using var reader = command.ExecuteReader();
			if (reader.Read())
			{
				return new Employee
				{
					Id = reader.GetInt32(0),
					Name = reader.GetString(1),
					Status = reader.GetInt32(2) == 1,
					Salary = reader.GetDecimal(3)
				};
			}
			return null;
		}		

		public void Update(Employee employee)
		{
			var query = "UPDATE Employees SET Name = @Name, Status = @Status, Salary = @Salary WHERE Id = @Id;";
			using var command = _connection.CreateCommand();
			command.CommandText = query;
			command.Parameters.AddWithValue("@Name", employee.Name);
			command.Parameters.AddWithValue("@Status", employee.Status ? 1 : 0);
			command.Parameters.AddWithValue("@Salary", employee.Salary);
			command.Parameters.AddWithValue("@Id", employee.Id);
			command.ExecuteNonQuery();
		}

		public void Delete(int id)
		{
			var query = "DELETE FROM Employees WHERE Id = @Id;";
			using var command = _connection.CreateCommand();
			command.CommandText = query;
			command.Parameters.AddWithValue("@Id", id);
			command.ExecuteNonQuery();
		}
	}
}
