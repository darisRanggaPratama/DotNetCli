using System.Globalization;
using System.Text;
using MySqlCLI.Data;
using MySqlCLI.Models;
using MySqlCLI.Views;

namespace MySqlCLI.Controllers;

public class EmployeeController
{
    private readonly DatabaseContext _dbContext;
    private readonly EmployeeView _view;

    public EmployeeController(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
        _view = new EmployeeView();
    }

    public void Run()
    {
        while (true)
        {
            _view.DisplayMainMenu();
            var choice = _view.GetMenuChoice();

            switch (choice[0])
            {
                case '1':
                    ShowAllEmployees();
                    break;
                case '2':
                    SearchEmployees();
                    break;
                case '3':
                    CreateEmployee();
                    break;
                case '4':
                    UpdateEmployee();
                    break;
                case '5':
                    DeleteEmployee();
                    break;
                case '6':
                    ImportFromCsv();
                    break;
                case '7':
                    ExportToCsv();
                    break;
                case '8':
                    return;
            }
        }
    }

    private void ShowAllEmployees()
    {
        var employees = _dbContext.GetAllEmployees();
        _view.DisplayEmployees(employees);
        _view.WaitForKeyPress();
    }

    private void SearchEmployees()
    {
        var criteria = _view.GetSearchCriteria();
        var employees = _dbContext.SearchEmployees(
            criteria.id,
            criteria.name,
            criteria.salaryFrom,
            criteria.salaryTo,
            criteria.status
        );

        Console.WriteLine();
        _view.DisplayEmployees(employees);
        _view.WaitForKeyPress();
    }

    private void CreateEmployee()
    {
        var employee = _view.GetNewEmployeeData();

        if (_dbContext.CreateEmployee(employee))
        {
            _view.ShowSuccessMessage("Karyawan berhasil ditambahkan!");
        }
        else
        {
            _view.ShowErrorMessage("Gagal menambahkan karyawan. ID mungkin sudah digunakan.");
        }

        _view.WaitForKeyPress();
    }

    private void UpdateEmployee()
    {
        var rowId = _view.GetRowIdForEdit();
        var existing = _dbContext.GetEmployeeByRowId(rowId);

        if (existing == null)
        {
            _view.ShowErrorMessage("Karyawan dengan Row ID tersebut tidak ditemukan.");
            _view.WaitForKeyPress();
            return;
        }

        var updated = _view.GetEditEmployeeData(existing);

        if (_dbContext.UpdateEmployee(updated))
        {
            _view.ShowSuccessMessage("Data karyawan berhasil diperbarui!");
        }
        else
        {
            _view.ShowErrorMessage("Gagal memperbarui data karyawan. ID mungkin sudah digunakan oleh karyawan lain.");
        }
        _view.WaitForKeyPress();
    }

    private void DeleteEmployee()
    {
        var rowId = _view.GetRowIdForDelete();
        var employee = _dbContext.GetEmployeeByRowId(rowId);

        if (employee == null)
        {
            _view.ShowErrorMessage("Karyawan dengan Row ID tersebut tidak ditemukan.");
            _view.WaitForKeyPress();
            return;
        }

        if (_view.ConfirmDelete(employee))
        {
            if (_dbContext.DeleteEmployee(rowId))
            {
                _view.ShowSuccessMessage("Karyawan berhasil dihapus!");
            }
            else
            {
                _view.ShowErrorMessage("Gagal menghapus karyawan.");
            }
        }
        else
        {
            _view.ShowSuccessMessage("Penghapusan dibatalkan.");
        }

        _view.WaitForKeyPress();
    }

    private void ImportFromCsv()
    {
        var filePath = _view.GetCsvFilePath(true);

        if (!File.Exists(filePath))
        {
            _view.ShowErrorMessage("File tidak ditemukan.");
            _view.WaitForKeyPress();
            return;
        }

        try
        {
            var employees = new List<Employee>();
            var lines = File.ReadAllLines(filePath, Encoding.UTF8);

            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(';');
                if (parts.Length != 4)
                {
                    _view.ShowErrorMessage($"Baris {i + 1} memiliki format yang salah. Dilewati.");
                    continue;
                }

                var id = parts[0].Trim();
                if (id.Length > 6)
                {
                    _view.ShowErrorMessage($"Baris {i + 1}: ID lebih dari 6 digit. Dilewati.");
                    continue;
                }

                var name = parts[1].Trim();

                if (!decimal.TryParse(parts[2].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var salary))
                {
                    _view.ShowErrorMessage($"Baris {i + 1}: Format gaji tidak valid. Dilewati.");
                    continue;
                }

                var statusStr = parts[3].Trim().ToLower();
                var status = statusStr == "1" || statusStr == "yes" || statusStr == "true";

                employees.Add(new Employee
                {
                    Id = id,
                    Name = name,
                    Salary = salary,
                    Status = status
                });
            }

            if (employees.Count > 0)
            {
                if (_dbContext.ImportFromCsv(employees))
                {
                    _view.ShowSuccessMessage($"Berhasil mengimport {employees.Count} data karyawan!");
                }
                else
                {
                    _view.ShowErrorMessage("Gagal mengimport data. Mungkin ada ID yang duplikat.");
                }
            }
            else
            {
                _view.ShowErrorMessage("Tidak ada data valid untuk diimport.");
            }
        }
        catch (Exception ex)
        {
            _view.ShowErrorMessage($"Error: {ex.Message}");
        }

        _view.WaitForKeyPress();
    }

    private void ExportToCsv()
    {
        var filePath = _view.GetCsvFilePath(false);

        try
        {
            var employees = _dbContext.GetAllEmployees();

            if (employees.Count == 0)
            {
                _view.ShowErrorMessage("Tidak ada data untuk diekspor.");
                _view.WaitForKeyPress();
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("id;name;salary;status");

            foreach (var emp in employees)
            {
                sb.AppendLine(
                    $"{emp.Id};{emp.Name};{emp.Salary.ToString(CultureInfo.InvariantCulture)};{(emp.Status ? "1" : "0")}");
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
            _view.ShowSuccessMessage($"Berhasil mengekspor {employees.Count} data karyawan ke {filePath}");
        }
        catch (Exception ex)
        {
            _view.ShowErrorMessage($"Error: {ex.Message}");
        }

        _view.WaitForKeyPress();
    }
}