using MySql.Data.MySqlClient;
using MySqlSpectreCLI.Models;

namespace MySqlSpectreCLI.Repositories
{
	public class EmployeeRepository
	{
		private readonly string _connectionString;

		public EmployeeRepository()
		{
			_connectionString = "Server=localhost;Database=db_sample;Uid=rangga;Pwd=rangga;";
		}

		private MySqlConnection GetConnection()
		{
			return new MySqlConnection(_connectionString);
		}

		public List<Employee> GetAll()
		{
			var employees = new List<Employee>();

			using var connection = GetConnection();
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
			using var connection = GetConnection();
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

		public bool Create(Employee employee)
		{
			try
			{
				using var connection = GetConnection();
				connection.Open();

				string query = "INSERT INTO employee (id, name, salary, status) VALUES (@id, @name, @salary, @status)";
				using var command = new MySqlCommand(query, connection);
				command.Parameters.AddWithValue("@id", employee.Id);
				command.Parameters.AddWithValue("@name", employee.Name);
				command.Parameters.AddWithValue("@salary", employee.Salary);
				command.Parameters.AddWithValue("@status", employee.Status ? 1 : 0);

				return command.ExecuteNonQuery() > 0;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool Update(Employee employee)
		{
			try
			{
				using var connection = GetConnection();
				connection.Open();

				string query = "UPDATE employee SET name = @name, salary = @salary, status = @status WHERE id = @id";
				using var command = new MySqlCommand(query, connection);
				command.Parameters.AddWithValue("@id", employee.Id);
				command.Parameters.AddWithValue("@name", employee.Name);
				command.Parameters.AddWithValue("@salary", employee.Salary);
				command.Parameters.AddWithValue("@status", employee.Status ? 1 : 0);

				return command.ExecuteNonQuery() > 0;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool Delete(string id)
		{
			try
			{
				using var connection = GetConnection();
				connection.Open();

				string query = "DELETE FROM employee WHERE id = @id";
				using var command = new MySqlCommand(query, connection);
				command.Parameters.AddWithValue("@id", id);

				return command.ExecuteNonQuery() > 0;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public List<Employee> Search(string? id = null, string? name = null, decimal? salary = null, bool? status = null)
		{
			var employees = new List<Employee>();

			using var connection = GetConnection();
			connection.Open();

			var query = "SELECT row_id, id, name, salary, status FROM employee WHERE 1=1";
			var parameters = new List<MySqlParameter>();

			if (!string.IsNullOrWhiteSpace(id))
			{
				query += " AND id LIKE @id";
				parameters.Add(new MySqlParameter("@id", $"%{id}%"));
			}

			if (!string.IsNullOrWhiteSpace(name))
			{
				query += " AND name LIKE @name";
				parameters.Add(new MySqlParameter("@name", $"%{name}%"));
			}

			if (salary.HasValue)
			{
				query += " AND salary = @salary";
				parameters.Add(new MySqlParameter("@salary", salary.Value));
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

		public bool BulkInsert(List<Employee> employees)
		{
			try
			{
				using var connection = GetConnection();
				connection.Open();

				using var transaction = connection.BeginTransaction();

				try
				{
					string query = "INSERT INTO employee (id, name, salary, status) VALUES (@id, @name, @salary, @status)";

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
				Status = reader.GetInt32("status") == 1
			};
		}
	}
}
