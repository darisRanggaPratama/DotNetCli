using CliAppMySqlCrud.Models;
using CliAppMySqlCrud.Services;
using CliAppMySqlCrud.Views;

namespace CliAppMySqlCrud.Controllers;

public class EmployeeController
{
    private readonly EmployeeService _employeeService;
    private readonly CsvService _csvService;
    private readonly ConsoleView _view;

    public EmployeeController(EmployeeService employeeService, CsvService csvService, ConsoleView view)
    {
        _employeeService = employeeService;
        _csvService = csvService;
        _view = view;
    }

    public void Run()
    {
        bool running = true;

        while (running)
        {
            _view.ShowTitle("EMPLOYEE MANAGEMENT");
            _view.ShowMenu();

            string choice = _view.GetInput("Pilih menu");

            switch (choice)
            {
                case "1":
                    ViewAllEmployees();
                    break;
                case "2":
                    CreateEmployee();
                    break;
                case "3":
                    UpdateEmployee();
                    break;
                case "4":
                    DeleteEmployee();
                    break;
                case "5":
                    FilterEmployees();
                    break;
                case "6":
                    ImportFromCsv();
                    break;
                case "7":
                    ExportToCsv();
                    break;
                case "0":
                    running = false;
                    _view.ShowInfo("Terima kasih telah menggunakan aplikasi ini!");
                    break;
                default:
                    _view.ShowError("Pilihan tidak valid!");
                    _view.WaitForKeyPress();
                    break;
            }
        }
    }

    private void ViewAllEmployees()
    {
        _view.ShowTitle("SEMUA DATA KARYAWAN");
        var employees = _employeeService.GetAllEmployees();
        _view.DisplayEmployees(employees);
        _view.WaitForKeyPress();
    }

    private void CreateEmployee()
    {
        _view.ShowTitle("TAMBAH KARYAWAN BARU");

        var employee = new Employee
        {
            Id = _view.GetInput("ID Karyawan (maks 6 digit)"),
            Name = _view.GetInput("Nama Karyawan"),
            Salary = _view.GetDecimalInput("Salary"),
            Status = _view.GetBoolInput("Status")
        };

        var (success, message) = _employeeService.CreateEmployee(employee);

        if (success)
        {
            _view.ShowSuccess(message);
        }
        else
        {
            _view.ShowError(message);
        }

        _view.WaitForKeyPress();
    }

    private void UpdateEmployee()
    {
        _view.ShowTitle("UPDATE DATA KARYAWAN");

        string id = _view.GetInput("Masukkan ID Karyawan yang akan diupdate");

        var existingEmployee = _employeeService.GetEmployeeById(id);

        if (existingEmployee == null)
        {
            _view.ShowError("Data karyawan tidak ditemukan!");
            _view.WaitForKeyPress();
            return;
        }

        _view.ShowInfo("Data karyawan saat ini:");
        _view.DisplayEmployees(new List<Employee> { existingEmployee });

        _view.ShowInfo("Masukkan data baru (tekan Enter untuk melewati kolom yang tidak ingin diubah)");

        string newName = _view.GetOptionalInput("Nama Karyawan", existingEmployee.Name);
        decimal newSalary = _view.GetOptionalDecimalInput("Salary", existingEmployee.Salary);
        bool newStatus = _view.GetOptionalBoolInput("Status", existingEmployee.Status);

        var updatedEmployee = new Employee
        {
            Id = id,
            Name = string.IsNullOrWhiteSpace(newName) ? existingEmployee.Name : newName,
            Salary = newSalary,
            Status = newStatus
        };

        var (success, message) = _employeeService.UpdateEmployee(updatedEmployee);

        if (success)
        {
            _view.ShowSuccess(message);
        }
        else
        {
            _view.ShowError(message);
        }

        _view.WaitForKeyPress();
    }

    private void DeleteEmployee()
    {
        _view.ShowTitle("HAPUS DATA KARYAWAN");

        string id = _view.GetInput("Masukkan ID Karyawan yang akan dihapus");

        var existingEmployee = _employeeService.GetEmployeeById(id);

        if (existingEmployee == null)
        {
            _view.ShowError("Data karyawan tidak ditemukan!");
            _view.WaitForKeyPress();
            return;
        }

        _view.ShowInfo("Data karyawan yang akan dihapus:");
        _view.DisplayEmployees(new List<Employee> { existingEmployee });

        if (_view.Confirm("Apakah Anda yakin ingin menghapus data ini?"))
        {
            var (success, message) = _employeeService.DeleteEmployee(id);

            if (success)
            {
                _view.ShowSuccess(message);
            }
            else
            {
                _view.ShowError(message);
            }
        }
        else
        {
            _view.ShowWarning("Penghapusan dibatalkan");
        }

        _view.WaitForKeyPress();
    }

    private void FilterEmployees()
    {
        _view.ShowTitle("FILTER DATA KARYAWAN");
        _view.ShowFilterMenu();

        string choice = _view.GetInput("Pilih filter");

        switch (choice)
        {
            case "1":
                FilterById();
                break;
            case "2":
                FilterByName();
                break;
            case "3":
                FilterBySalary();
                break;
            case "4":
                FilterByStatus();
                break;
            case "0":
                return;
            default:
                _view.ShowError("Pilihan tidak valid!");
                _view.WaitForKeyPress();
                break;
        }
    }

    private void FilterById()
    {
        _view.ShowTitle("FILTER BERDASARKAN ID");

        string id = _view.GetInput("Masukkan ID Karyawan");
        var employee = _employeeService.GetEmployeeById(id);

        if (employee != null)
        {
            _view.DisplayEmployees(new List<Employee> { employee }, $"HASIL PENCARIAN - ID: {id}");
        }
        else
        {
            _view.ShowWarning($"Karyawan dengan ID {id} tidak ditemukan");
        }

        _view.WaitForKeyPress();
    }

    private void FilterByName()
    {
        _view.ShowTitle("FILTER BERDASARKAN NAMA");

        string name = _view.GetInput("Masukkan Nama Karyawan (bisa sebagian)");
        var employees = _employeeService.SearchEmployeesByName(name);

        _view.DisplayEmployees(employees, $"HASIL PENCARIAN - Nama: {name}");
        _view.WaitForKeyPress();
    }

    private void FilterBySalary()
    {
        _view.ShowTitle("FILTER BERDASARKAN SALARY");

        decimal minSalary = _view.GetDecimalInput("Salary Minimum");
        decimal maxSalary = _view.GetDecimalInput("Salary Maksimum");

        var employees = _employeeService.FilterEmployeesBySalary(minSalary, maxSalary);

        _view.DisplayEmployees(employees, $"HASIL PENCARIAN - Salary: {minSalary:N2} - {maxSalary:N2}");
        _view.WaitForKeyPress();
    }

    private void FilterByStatus()
    {
        _view.ShowTitle("FILTER BERDASARKAN STATUS");

        bool status = _view.GetBoolInput("Status yang dicari");
        var employees = _employeeService.FilterEmployeesByStatus(status);

        string statusText = status ? "Active (1)" : "Inactive (0)";
        _view.DisplayEmployees(employees, $"HASIL PENCARIAN - Status: {statusText}");
        _view.WaitForKeyPress();
    }

    private void ImportFromCsv()
    {
        _view.ShowTitle("IMPORT DATA DARI CSV");

        _view.ShowInfo("File CSV harus berada di Desktop");
        _view.ShowInfo("Format: id;name;salary;status");
        _view.ShowInfo("Baris pertama (header) akan diabaikan");

        string filename = _view.GetInput("Nama file CSV (contoh: employees.csv)");

        var (success, message, imported) = _csvService.ImportFromCsv(filename);

        if (success)
        {
            _view.ShowSuccess(message);
        }
        else
        {
            _view.ShowError(message);
        }

        _view.WaitForKeyPress();
    }

    private void ExportToCsv()
    {
        _view.ShowTitle("EXPORT DATA KE CSV");

        _view.ShowInfo("File CSV akan disimpan di Desktop");

        string filename = _view.GetInput("Nama file CSV (contoh: employees_export.csv)");

        var (success, message) = _csvService.ExportToCsv(filename);

        if (success)
        {
            _view.ShowSuccess(message);
        }
        else
        {
            _view.ShowError(message);
        }

        _view.WaitForKeyPress();
    }
}