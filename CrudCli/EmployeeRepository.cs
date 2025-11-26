namespace CrudCli;

public class EmployeeRepository: IEmployeeRepository
{
    private readonly Dictionary<int, Employee> _employees;
    private int _nextId;

    public EmployeeRepository()
    {
        _employees = new Dictionary<int, Employee>();
        _nextId = 1;
    }
    
    public void Create(Employee employee)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee));
        
        employee.Id = _nextId++;
        _employees.Add(employee.Id, employee);
    }

    public Employee? Read(int id)
    {
        return _employees.TryGetValue(id, out var employee) ? employee : null;
    }

    public IEnumerable<Employee> ReadAll()
    {
       return _employees.Values.ToList();
    }

    public bool Update(int id, Employee employee)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee));
        
        if (!_employees.ContainsKey(id)) return false;
        
        employee.Id = id;
        _employees[id] = employee;
        return true;
    }

    public bool Delete(int id)
    {
        return _employees.Remove(id);
    }
}