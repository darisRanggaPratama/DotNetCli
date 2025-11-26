namespace CrudCli;

public class EmployeeManagementApp
{
    private readonly EmployeeService _service;
    private bool _isRunning;

    public EmployeeManagementApp()
    {
        EmployeeRepository repository = new EmployeeRepository();
        _service = new EmployeeService(repository);
        _isRunning = true;
    }

    public void Run()
    {
        Print.Text($"Employee Management Application");
        while (_isRunning)
        {
            DisplayMenu();
            ProcessUserChoice();
        }
    }

    private void DisplayMenu()
    {
        Print.Text($"--Menu:--");
        Print.Text($"1. Add Employee");
        Print.Text($"2. View Employee by ID");
        Print.Text($"3. View All Employees");
        Print.Text($"4. Update Employee");
        Print.Text($"5. Delete Employee");
        Print.Text($"6. Exit");
        Print.Text("Please enter your choice (1-6): ");
    }

    private void ProcessUserChoice()
    {
        try
        {
            string? choice = Console.ReadLine();
            Print.Text($"");
            
            switch (choice)
            {
                case "1":
                    HandleAddEmployee();
                    break;
                case "2":
                    HandleViewEmployee();
                    break;
                case "3":
                    _service.DisplayAllEmployees();
                    break;
                case "4":
                    HandleUpdateEmployee();
                    break;
                case "5":
                    HandleDeleteEmployee();
                    break;
                case "6":
                    _isRunning = false;
                    Print.Text("Exiting the application. Goodbye!");
                    break;
                default:
                    Print.Text("Invalid choice. Please enter a number between 1 and 6.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Print.Text("Error: " + ex.Message);
        }
    }

    private void HandleDeleteEmployee()
    {
        Print.Text($"Enter Employee ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _service.RemoveEmployee(id);
        }
        else
        {
            Print.Text("Invalid ID input. Please enter a valid integer.");
        }
    }

    private void HandleUpdateEmployee()
    {
        Print.Text("Enter Employee ID to update: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Print.Text("Enter new Employee Name: ");
            string? name = Console.ReadLine();
            
            Print.Text("Enter new Employee Salary: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal salary))
            {
                Print.Text("Invalid salary input. Please enter a valid decimal number.");
            }
            
            Print.Text("Is Employee Active? (yes/no): ");
            string? statusInput = Console.ReadLine()?.ToLower();
            bool status = statusInput == "yes" || statusInput == "y";
            
            if (name != null) _service.UpdateEmployee(id, name, salary, status);
        }
        else
        {
            Print.Text("Invalid ID input. Please enter a valid integer.");
        }
    }

    private void HandleViewEmployee()
    {
        Print.Text($"Enter Employee ID to view: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _service.DisplayEmployee(id);
        }
        else
        {
            Print.Text("Invalid ID input. Please enter a valid integer.");
        }
    }

    private void HandleAddEmployee()
    {
        Print.Text($"Enter Employee Name: ");
        string? name = Console.ReadLine();
        
        Print.Text($"Enter Employee Salary: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal salary))
        {
            Print.Text("Invalid salary input. Please enter a valid decimal number.");
        }
        
        Print.Text($"Is Employee Active? (yes/no): ");
        string? statusInput = Console.ReadLine()?.ToLower();
        bool status = statusInput == "yes" || statusInput == "y";
        
        if (name != null) _service.AddEmployee(name, salary, status);
    }
}