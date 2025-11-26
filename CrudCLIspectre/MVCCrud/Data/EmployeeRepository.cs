using Microsoft.Data.Sqlite;
using EmployeeManagement.Models;

namespace EmployeeManagement.Data;

public class EmployeeRepository : IDisposable
{
	private readonly string _connectionString;
	private SqliteConnection? _connection;

	public EmployeeRepository(string dbPath = "employees.db")
	{
		_connectionString = $"Data Source={dbPath}";
		InitializeDatabase();
	}

	private void InitializeDatabase()
	{
		using var connection = new SqliteConnection(_connectionString);
		connection.Open();

		var command = connection.CreateCommand();
		command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Employees (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Salary REAL NOT NULL,
                Status INTEGER NOT NULL
            )";
		command.ExecuteNonQuery();
	}

	private SqliteConnection GetConnection()
	{
		if (_connection == null || _connection.State != System.Data.ConnectionState.Open)
		{
			_connection = new SqliteConnection(_connectionString);
			_connection.Open();
		}
		return _connection;
	}

	public void Create(Employee employee)
	{
		var connection = GetConnection();
		var command = connection.CreateCommand();
		command.CommandText = @"
            INSERT INTO Employees (Name, Salary, Status)
            VALUES ($name, $salary, $status)";

		command.Parameters.AddWithValue("$name", employee.Name);
		command.Parameters.AddWithValue("$salary", employee.Salary);
		command.Parameters.AddWithValue("$status", employee.Status ? 1 : 0);

		command.ExecuteNonQuery();
	}

	public List<Employee> GetAll()
	{
		var employees = new List<Employee>();
		var connection = GetConnection();
		var command = connection.CreateCommand();
		command.CommandText = "SELECT Id, Name, Salary, Status FROM Employees";

		using var reader = command.ExecuteReader();
		while (reader.Read())
		{
			employees.Add(MapToEmployee(reader));
		}

		return employees;
	}

	public Employee? GetById(int id)
	{
		var connection = GetConnection();
		var command = connection.CreateCommand();
		command.CommandText = "SELECT Id, Name, Salary, Status FROM Employees WHERE Id = $id";
		command.Parameters.AddWithValue("$id", id);

		using var reader = command.ExecuteReader();
		if (reader.Read())
		{
			return MapToEmployee(reader);
		}

		return null;
	}

	public List<Employee> GetByName(string name)
	{
		var employees = new List<Employee>();
		var connection = GetConnection();
		var command = connection.CreateCommand();
		command.CommandText = "SELECT Id, Name, Salary, Status FROM Employees WHERE Name LIKE $name";
		command.Parameters.AddWithValue("$name", $"%{name}%");

		using var reader = command.ExecuteReader();
		while (reader.Read())
		{
			employees.Add(MapToEmployee(reader));
		}

		return employees;
	}

	public List<Employee> GetBySalary(decimal minSalary, decimal maxSalary)
	{
		var employees = new List<Employee>();
		var connection = GetConnection();
		var command = connection.CreateCommand();
		command.CommandText = "SELECT Id, Name, Salary, Status FROM Employees WHERE Salary BETWEEN $min AND $max";
		command.Parameters.AddWithValue("$min", minSalary);
		command.Parameters.AddWithValue("$max", maxSalary);

		using var reader = command.ExecuteReader();
		while (reader.Read())
		{
			employees.Add(MapToEmployee(reader));
		}

		return employees;
	}

	public List<Employee> GetByStatus(bool status)
	{
		var employees = new List<Employee>();
		var connection = GetConnection();
		var command = connection.CreateCommand();
		command.CommandText = "SELECT Id, Name, Salary, Status FROM Employees WHERE Status = $status";
		command.Parameters.AddWithValue("$status", status ? 1 : 0);

		using var reader = command.ExecuteReader();
		while (reader.Read())
		{
			employees.Add(MapToEmployee(reader));
		}

		return employees;
	}

	public void Update(Employee employee)
	{
		var connection = GetConnection();
		var command = connection.CreateCommand();
		command.CommandText = @"
            UPDATE Employees 
            SET Name = $name, Salary = $salary, Status = $status
            WHERE Id = $id";

		command.Parameters.AddWithValue("$id", employee.Id);
		command.Parameters.AddWithValue("$name", employee.Name);
		command.Parameters.AddWithValue("$salary", employee.Salary);
		command.Parameters.AddWithValue("$status", employee.Status ? 1 : 0);

		command.ExecuteNonQuery();
	}

	public void Delete(int id)
	{
		var connection = GetConnection();
		var command = connection.CreateCommand();
		command.CommandText = "DELETE FROM Employees WHERE Id = $id";
		command.Parameters.AddWithValue("$id", id);
		command.ExecuteNonQuery();
	}

	private Employee MapToEmployee(SqliteDataReader reader)
	{
		return new Employee
		{
			Id = reader.GetInt32(0),
			Name = reader.GetString(1),
			Salary = reader.GetDecimal(2),
			Status = reader.GetInt32(3) == 1
		};
	}

	public void Dispose()
	{
		_connection?.Dispose();
	}
}