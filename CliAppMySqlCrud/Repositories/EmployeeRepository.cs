using CliAppMySqlCrud.Data;
using CliAppMySqlCrud.Models;
using MySql.Data.MySqlClient;

namespace CliAppMySqlCrud.Repositories;

public class EmployeeRepository
{
    private readonly DatabaseConnection _dbConnection;

    public EmployeeRepository(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public List<Employee> GetAll()
    {
        var employees = new List<Employee>();
        using var connection = _dbConnection.GetConnection();
        connection.Open();

        string query = "SELECT row_id, id, name, salary, status FROM employee ORDER BY row_id";
        using var command = new MySqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            employees.Add(MapToEmployee(reader));
        }

        return employees;
    }

    public Employee? GetById(string id)
    {
        using var connection = _dbConnection.GetConnection();
        connection.Open();

        string query = "SELECT row_id, id, name, salary, status FROM employee WHERE id = @id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return MapToEmployee(reader);
        }

        return null;
    }

    public List<Employee> SearchByName(string name)
    {
        var employees = new List<Employee>();
        using var connection = _dbConnection.GetConnection();
        connection.Open();

        string query = "SELECT row_id, id, name, salary, status FROM employee WHERE name LIKE @name ORDER BY row_id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@name", $"%{name}%");
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            employees.Add(MapToEmployee(reader));
        }

        return employees;
    }

    public List<Employee> FilterBySalary(decimal minSalary, decimal maxSalary)
    {
        var employees = new List<Employee>();
        using var connection = _dbConnection.GetConnection();
        connection.Open();

        string query =
            "SELECT row_id, id, name, salary, status FROM employee WHERE salary BETWEEN @min AND @max ORDER BY row_id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@min", minSalary);
        command.Parameters.AddWithValue("@max", maxSalary);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            employees.Add(MapToEmployee(reader));
        }

        return employees;
    }

    public List<Employee> FilterByStatus(bool status)
    {
        var employees = new List<Employee>();
        using var connection = _dbConnection.GetConnection();
        connection.Open();

        string query = "SELECT row_id, id, name, salary, status FROM employee WHERE status = @status ORDER BY row_id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@status", status ? 1 : 0);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            employees.Add(MapToEmployee(reader));
        }

        return employees;
    }

    public bool Create(Employee employee)
    {
        try
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "INSERT INTO employee (id, name, salary, status) VALUES (@id, @name, @salary, @status)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", employee.Id);
            command.Parameters.AddWithValue("@name", employee.Name);
            command.Parameters.AddWithValue("@salary", employee.Salary);
            command.Parameters.AddWithValue("@status", employee.Status ? 1 : 0);

            return command.ExecuteNonQuery() > 0;
        }
        catch
        {
            return false;
        }
    }

    public bool Update(Employee employee)
    {
        try
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "UPDATE employee SET name = @name, salary = @salary, status = @status WHERE id = @id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", employee.Id);
            command.Parameters.AddWithValue("@name", employee.Name);
            command.Parameters.AddWithValue("@salary", employee.Salary);
            command.Parameters.AddWithValue("@status", employee.Status ? 1 : 0);

            return command.ExecuteNonQuery() > 0;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(string id)
    {
        try
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "DELETE FROM employee WHERE id = @id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            return command.ExecuteNonQuery() > 0;
        }
        catch
        {
            return false;
        }
    }

    public bool DeleteAll()
    {
        try
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            string query = "DELETE FROM employee";
            using var command = new MySqlCommand(query, connection);

            command.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private Employee MapToEmployee(MySqlDataReader reader)
    {
        return new Employee
        {
            RowId = reader.GetInt32("row_id"),
            Id = reader.GetString("id"),
            Name = reader.GetString("name"),
            Salary = reader.GetDecimal("salary"),
            Status = reader.GetBoolean("status")
        };
    }
}