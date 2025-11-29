using System.Data;
using MySql.Data.MySqlClient;
using MySqlCLIapp.Data;
using MySqlCLIapp.Models;

namespace MySqlCLIapp.Repositories
{
    public class MySqlEmployeeRepository : IEmployeeRepository
    {
        public IEnumerable<Employee> GetAll()
        {
            var list = new List<Employee>();
            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("SELECT row_id, id, name, salary, status FROM employee ORDER BY row_id", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }
            return list;
        }

        public Employee? GetById(string id)
        {
            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("SELECT row_id, id, name, salary, status FROM employee WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? Map(reader) : null;
        }

        public IEnumerable<Employee> GetByName(string nameContains)
        {
            var list = new List<Employee>();
            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("SELECT row_id, id, name, salary, status FROM employee WHERE name LIKE @name ORDER BY name", conn);
            cmd.Parameters.AddWithValue("@name", $"%{nameContains}%");
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(Map(reader));
            return list;
        }

        public IEnumerable<Employee> GetBySalary(decimal salary)
        {
            var list = new List<Employee>();
            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("SELECT row_id, id, name, salary, status FROM employee WHERE salary = @salary ORDER BY name", conn);
            cmd.Parameters.AddWithValue("@salary", salary);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(Map(reader));
            return list;
        }

        public IEnumerable<Employee> GetByStatus(int status)
        {
            var list = new List<Employee>();
            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("SELECT row_id, id, name, salary, status FROM employee WHERE status = @status ORDER BY name", conn);
            cmd.Parameters.AddWithValue("@status", status);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(Map(reader));
            return list;
        }

        public void Create(Employee emp)
        {
            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("INSERT INTO employee (id, name, salary, status) VALUES (@id, @name, @salary, @status)", conn);
            cmd.Parameters.AddWithValue("@id", emp.Id);
            cmd.Parameters.AddWithValue("@name", emp.Name);
            cmd.Parameters.AddWithValue("@salary", emp.Salary);
            cmd.Parameters.AddWithValue("@status", emp.Status);
            cmd.ExecuteNonQuery();
        }

        public bool Update(Employee emp)
        {
            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("UPDATE employee SET name = @name, salary = @salary, status = @status WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", emp.Id);
            cmd.Parameters.AddWithValue("@name", emp.Name);
            cmd.Parameters.AddWithValue("@salary", emp.Salary);
            cmd.Parameters.AddWithValue("@status", emp.Status);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(string id)
        {
            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("DELETE FROM employee WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            return cmd.ExecuteNonQuery() > 0;
        }

        public void BulkUpsert(IEnumerable<Employee> employees)
        {
            using var conn = Database.GetConnection();
            conn.Open();
            using var tx = conn.BeginTransaction();
            foreach (var emp in employees)
            {
                using var cmd = new MySqlCommand(
                    "INSERT INTO employee (id, name, salary, status) VALUES (@id,@name,@salary,@status) " +
                    "ON DUPLICATE KEY UPDATE name=VALUES(name), salary=VALUES(salary), status=VALUES(status)", conn, tx);
                cmd.Parameters.AddWithValue("@id", emp.Id);
                cmd.Parameters.AddWithValue("@name", emp.Name);
                cmd.Parameters.AddWithValue("@salary", emp.Salary);
                cmd.Parameters.AddWithValue("@status", emp.Status);
                cmd.ExecuteNonQuery();
            }
            tx.Commit();
        }

        private static Employee Map(IDataRecord r)
        {
            return new Employee
            {
                RowId = r.GetInt32(0),
                Id = r.GetString(1),
                Name = r.GetString(2),
                Salary = r.GetDecimal(3),
                Status = r.GetInt32(4)
            };
        }
    }
}