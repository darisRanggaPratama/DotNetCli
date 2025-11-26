using CrudSpectreSearch.Models;
using Microsoft.Data.Sqlite;

namespace CrudSpectreSearch.Services
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
			var query = "INSERT INTO Employees (Name, Salary, Status) VALUES (@Name, @Salary, @Status)";
			using var command = _connection.CreateCommand();
			command.CommandText = query;
			command.Parameters.AddWithValue("@Name", employee.Name);
			command.Parameters.AddWithValue("@Salary", employee.Salary);
			command.Parameters.AddWithValue("@Status", employee.Status ? 1 : 0);
			command.ExecuteNonQuery();
		}

		public IEnumerable<Employee> GetAll()
		{
			var employees = new List<Employee>();
			var query = "SELECT Id, Name, Salary, Status FROM Employees ORDER BY Id";

			using var command = _connection.CreateCommand();
			command.CommandText = query;

			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				employees.Add(new Employee
				{
					Id = reader.GetInt32(0),
					Name = reader.GetString(1),
					Salary = reader.GetDecimal(2),
					Status = reader.GetInt32(3) == 1
				});
			}

			return employees;
		}

		public IEnumerable<Employee> SearchByName(string name)
		{
			var employees = new List<Employee>();
			var query = "SELECT Id, Name, Salary, Status FROM Employees WHERE Name LIKE @Name ORDER BY Id";

			using var command = _connection.CreateCommand();
			command.CommandText = query;
			command.Parameters.AddWithValue("@Name", $"%{name}%");

			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				employees.Add(new Employee
				{
					Id = reader.GetInt32(0),
					Name = reader.GetString(1),
					Salary = reader.GetDecimal(2),
					Status = reader.GetInt32(3) == 1
				});
			}

			return employees;
		}

		public IEnumerable<Employee> SearchBySalaryRange(decimal minSalary, decimal maxSalary)
		{
			var employees = new List<Employee>();
			var query = "SELECT Id, Name, Salary, Status FROM Employees WHERE Salary BETWEEN @MinSalary AND @MaxSalary ORDER BY Salary";

			using var command = _connection.CreateCommand();
			command.CommandText = query;
			command.Parameters.AddWithValue("@MinSalary", minSalary);
			command.Parameters.AddWithValue("@MaxSalary", maxSalary);

			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				employees.Add(new Employee
				{
					Id = reader.GetInt32(0),
					Name = reader.GetString(1),
					Salary = reader.GetDecimal(2),
					Status = reader.GetInt32(3) == 1
				});
			}

			return employees;
		}

		public IEnumerable<Employee> SearchByStatus(bool status)
		{
			var employees = new List<Employee>();
			var query = "SELECT Id, Name, Salary, Status FROM Employees WHERE Status = @Status ORDER BY Id";

			using var command = _connection.CreateCommand();
			command.CommandText = query;
			command.Parameters.AddWithValue("@Status", status ? 1 : 0);

			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				employees.Add(new Employee
				{
					Id = reader.GetInt32(0),
					Name = reader.GetString(1),
					Salary = reader.GetDecimal(2),
					Status = reader.GetInt32(3) == 1
				});
			}

			return employees;
		}

		public Employee? GetById(int id)
		{
			var query = "SELECT Id, Name, Salary, Status FROM Employees WHERE Id = @Id";
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
					Salary = reader.GetDecimal(2),
					Status = reader.GetInt32(3) == 1
				};
			}

			return null;
		}

		public void Update(Employee employee)
		{
			var query = "UPDATE Employees SET Name = @Name, Salary = @Salary, Status = @Status WHERE Id = @Id";
			using var command = _connection.CreateCommand();
			command.CommandText = query;
			command.Parameters.AddWithValue("@Id", employee.Id);
			command.Parameters.AddWithValue("@Name", employee.Name);
			command.Parameters.AddWithValue("@Salary", employee.Salary);
			command.Parameters.AddWithValue("@Status", employee.Status ? 1 : 0);
			command.ExecuteNonQuery();
		}

		public void Delete(int id)
		{
			var query = "DELETE FROM Employees WHERE Id = @Id";
			using var command = _connection.CreateCommand();
			command.CommandText = query;
			command.Parameters.AddWithValue("@Id", id);
			command.ExecuteNonQuery();
		}
	}
}
