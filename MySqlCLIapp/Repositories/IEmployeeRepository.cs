using MySqlCLIapp.Models;

namespace MySqlCLIapp.Repositories
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
        Employee? GetById(string id);
        IEnumerable<Employee> GetByName(string nameContains);
        IEnumerable<Employee> GetBySalary(decimal salary);
        IEnumerable<Employee> GetByStatus(int status);
        void Create(Employee emp);
        bool Update(Employee emp); // based on Id
        bool Delete(string id);
        void BulkUpsert(IEnumerable<Employee> employees);
    }
}