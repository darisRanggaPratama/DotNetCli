using System.Data;
using MySql.Data.MySqlClient;
using MySqlCLI.Models;

namespace MySqlCLI.Data;

public class DatabaseContext
{
    private readonly string _connectionString;

    public DatabaseContext(string connectionString)
    {
        _connectionString = connectionString;
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var createTableQuery = @"
                CREATE TABLE IF NOT EXISTS employee (
                    row_id INT AUTO_INCREMENT PRIMARY KEY,
                    id VARCHAR(6) NOT NULL UNIQUE,
                    name VARCHAR(255) NOT NULL,
                    salary DECIMAL(15,2) NOT NULL,
                    status TINYINT(1) NOT NULL DEFAULT 1,
                    INDEX idx_id (id),
                    INDEX idx_name (name),
                    INDEX idx_salary (salary),
                    INDEX idx_status (status)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;";

        using var command = new MySqlCommand(createTableQuery, connection);
        command.ExecuteNonQuery();
    }

    public List<Employee> GetAllEmployees()
    {
        var employees = new List<Employee>();

        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT row_id, id, name, salary, status FROM employee ORDER BY row_id";
        using var command = new MySqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            employees.Add(MapToEmployee(reader));
        }

        return employees;
    }

    public Employee? GetEmployeeById(string id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT row_id, id, name, salary, status FROM employee WHERE id = @id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return MapToEmployee(reader);
        }

        return null;
    }

    public List<Employee> SearchEmployees(string? id, string? name, decimal? salaryFrom, decimal? salaryTo,
        bool? status)
    {
        var employees = new List<Employee>();

        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT row_id, id, name, salary, status FROM employee WHERE 1=1";
        var parameters = new List<MySqlParameter>();

        if (!string.IsNullOrEmpty(id))
        {
            query += " AND id LIKE @id";
            parameters.Add(new MySqlParameter("@id", $"%{id}%"));
        }

        if (!string.IsNullOrEmpty(name))
        {
            query += " AND name LIKE @name";
            parameters.Add(new MySqlParameter("@name", $"%{name}%"));
        }

        if (salaryFrom.HasValue)
        {
            query += " AND salary >= @salaryFrom";
            parameters.Add(new MySqlParameter("@salaryFrom", salaryFrom.Value));
        }

        if (salaryTo.HasValue)
        {
            query += " AND salary <= @salaryTo";
            parameters.Add(new MySqlParameter("@salaryTo", salaryTo.Value));
        }

        if (status.HasValue)
        {
            query += " AND status = @status";
            parameters.Add(new MySqlParameter("@status", status.Value ? 1 : 0));
        }

        query += " ORDER BY row_id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddRange(parameters.ToArray());

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            employees.Add(MapToEmployee(reader));
        }

        return employees;
    }

    public bool CreateEmployee(Employee employee)
    {
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var query = "INSERT INTO employee (id, name, salary, status) VALUES (@id, @name, @salary, @status)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", employee.Id);
            command.Parameters.AddWithValue("@name", employee.Name);
            command.Parameters.AddWithValue("@salary", employee.Salary);
            command.Parameters.AddWithValue("@status", employee.Status ? 1 : 0);

            return command.ExecuteNonQuery() > 0;
        }
        catch (MySqlException)
        {
            return false;
        }
    }

    public bool UpdateEmployee(string oldId, Employee employee)
    {
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var query =
                "UPDATE employee SET id = @newId, name = @name, salary = @salary, status = @status WHERE id = @oldId";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@oldId", oldId);
            command.Parameters.AddWithValue("@newId", employee.Id);
            command.Parameters.AddWithValue("@name", employee.Name);
            command.Parameters.AddWithValue("@salary", employee.Salary);
            command.Parameters.AddWithValue("@status", employee.Status ? 1 : 0);

            return command.ExecuteNonQuery() > 0;
        }
        catch (MySqlException)
        {
            return false;
        }
    }

    public bool DeleteEmployee(string id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "DELETE FROM employee WHERE id = @id";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        return command.ExecuteNonQuery() > 0;
    }

    public bool ImportFromCsv(List<Employee> employees)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        using var transaction = connection.BeginTransaction();
        try
        {
            var query = "INSERT INTO employee (id, name, salary, status) VALUES (@id, @name, @salary, @status)";

            foreach (var employee in employees)
            {
                using var command = new MySqlCommand(query, connection, transaction);
                command.Parameters.AddWithValue("@id", employee.Id);
                command.Parameters.AddWithValue("@name", employee.Name);
                command.Parameters.AddWithValue("@salary", employee.Salary);
                command.Parameters.AddWithValue("@status", employee.Status ? 1 : 0);
                command.ExecuteNonQuery();
            }

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            return false;
        }
    }

    private Employee MapToEmployee(IDataReader reader)
    {
        return new Employee
        {
            RowId = reader.GetInt32(0),
            Id = reader.GetString(1),
            Name = reader.GetString(2),
            Salary = reader.GetDecimal(3),
            Status = reader.GetBoolean(4)
        };
    }
}