namespace CrudCli;

public class EmployeeService
{
    private readonly IEmployeeRepository _repository;
    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    public void AddEmployee(string name, decimal salary, bool status)
    {
        ValidateEmployeeData(name, salary);
        Employee employee = new Employee(0, name, salary, status);
        _repository.Create(employee);
        Print.Text($"\nEmployee added successfully. with ID: {employee.Id}\n");
    }

    public void DisplayEmployee(int id)
    {
        Employee employee = _repository.Read(id);
        if (employee == null)
        {
            Print.Text($"\nEmployee with ID {id} not found.\n");
            return;
        }
        else
        {
            Print.Text($"\n{employee}\n");
        }
    }
    
    public void DisplayAllEmployees()
    {
        IEnumerable<Employee> employees = _repository.ReadAll();
        if (!employees.Any())
        {
            Print.Text("\nNo employees found.\n");
            return;
        }

        Print.Text("\nAll Employee List:");
        foreach (Employee employee in employees)
        {
            Print.Text(employee.ToString());
        }
        Print.Text("");
    }
    
    public void UpdateEmployee(int id, string name, decimal salary, bool status)
    {
        ValidateEmployeeData(name, salary);
        Employee employee = new Employee(id, name, salary, status);
        bool isUpdated = _repository.Update(id, employee);
        if (isUpdated)
        {
            Print.Text($"\nEmployee with ID {id} updated successfully.\n");
        }
        else
        {
            Print.Text($"\nEmployee with ID {id} not found.\n");
        }
    }

    public void RemoveEmployee(int id)
    {
        bool isDeleted = _repository.Delete(id);
        if (isDeleted)
        {
            Print.Text($"\nEmployee with ID {id} deleted successfully.\n");
        }
        else
        {
            Print.Text($"\nEmployee with ID {id} not found.\n");
        }
    }
    
    private void ValidateEmployeeData(string name, decimal salary)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        
        if (salary < 0)
            throw new ArgumentException(nameof(salary), $"Salary cannot be negative.");
    }
}