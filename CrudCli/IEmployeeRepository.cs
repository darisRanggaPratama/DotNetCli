namespace CrudCli;

public interface IEmployeeRepository
{
    void Create(Employee employee);
    Employee? Read(int id);
    IEnumerable<Employee> ReadAll();
    bool Update(int id, Employee employee);
    bool Delete(int id);
}
