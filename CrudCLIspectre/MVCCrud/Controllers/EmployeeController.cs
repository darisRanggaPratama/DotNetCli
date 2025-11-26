using EmployeeManagement.Data;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using EmployeeManagement.Views;

namespace EmployeeManagement.Controllers;

public class EmployeeController : IDisposable
{
	private readonly EmployeeRepository _repository;
	private readonly EmployeeView _view;
	private readonly CsvService _csvService;
	private bool _disposed = false;

	public EmployeeController()
	{
		_repository = new EmployeeRepository();
		_view = new EmployeeView();
		_csvService = new CsvService();
	}

	public void Run()
	{
		bool exit = false;

		while (!exit)
		{
			var choice = _view.ShowMainMenu();

			switch (choice)
			{
				case "1":
					CreateEmployee();
					break;
				case "2":
					ViewAllEmployees();
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
					exit = true;
					_view.ShowExitMessage();
					break;
				default:
					_view.ShowErrorMessage("Pilihan tidak valid!");
					break;
			}

			if (!exit)
			{
				_view.PressAnyKey();
			}
		}
	}

	private void CreateEmployee()
	{
		_view.ShowCreateHeader();
		var employee = _view.GetEmployeeInput();

		try
		{
			_repository.Create(employee);
			_view.ShowSuccessMessage("Data karyawan berhasil ditambahkan!");
		}
		catch (Exception ex)
		{
			_view.ShowErrorMessage($"Gagal menambahkan data: {ex.Message}");
		}
	}

	private void ViewAllEmployees()
	{
		try
		{
			var employees = _repository.GetAll();
			_view.DisplayEmployees(employees, "Semua Data Karyawan");
		}
		catch (Exception ex)
		{
			_view.ShowErrorMessage($"Gagal mengambil data: {ex.Message}");
		}
	}

	private void UpdateEmployee()
	{
		_view.ShowUpdateHeader();
		var id = _view.GetEmployeeId();

		try
		{
			var employee = _repository.GetById(id);

			if (employee == null)
			{
				_view.ShowErrorMessage("Karyawan dengan ID tersebut tidak ditemukan!");
				return;
			}

			_view.DisplaySingleEmployee(employee);
			var updatedEmployee = _view.GetEmployeeInput();
			updatedEmployee.Id = id;

			_repository.Update(updatedEmployee);
			_view.ShowSuccessMessage("Data karyawan berhasil diupdate!");
		}
		catch (Exception ex)
		{
			_view.ShowErrorMessage($"Gagal mengupdate data: {ex.Message}");
		}
	}

	private void DeleteEmployee()
	{
		_view.ShowDeleteHeader();
		var id = _view.GetEmployeeId();

		try
		{
			var employee = _repository.GetById(id);

			if (employee == null)
			{
				_view.ShowErrorMessage("Karyawan dengan ID tersebut tidak ditemukan!");
				return;
			}

			_view.DisplaySingleEmployee(employee);

			if (_view.ConfirmDelete())
			{
				_repository.Delete(id);
				_view.ShowSuccessMessage("Data karyawan berhasil dihapus!");
			}
			else
			{
				_view.ShowInfoMessage("Penghapusan dibatalkan.");
			}
		}
		catch (Exception ex)
		{
			_view.ShowErrorMessage($"Gagal menghapus data: {ex.Message}");
		}
	}

	private void FilterEmployees()
	{
		var filterType = _view.ShowFilterMenu();
		List<Employee> employees = new();

		try
		{
			switch (filterType)
			{
				case "1":
					var name = _view.GetSearchName();
					employees = _repository.GetByName(name);
					_view.DisplayEmployees(employees, $"Hasil Pencarian: Nama mengandung '{name}'");
					break;
				case "2":
					var (minSalary, maxSalary) = _view.GetSalaryRange();
					employees = _repository.GetBySalary(minSalary, maxSalary);
					_view.DisplayEmployees(employees, $"Hasil Pencarian: Gaji antara {minSalary:N0} - {maxSalary:N0}");
					break;
				case "3":
					var status = _view.GetStatusFilter();
					employees = _repository.GetByStatus(status);
					_view.DisplayEmployees(employees, $"Hasil Pencarian: Status {(status ? "Yes" : "No")}");
					break;
				default:
					_view.ShowErrorMessage("Pilihan tidak valid!");
					break;
			}
		}
		catch (Exception ex)
		{
			_view.ShowErrorMessage($"Gagal melakukan pencarian: {ex.Message}");
		}
	}

	private void ImportFromCsv()
	{
		_view.ShowImportHeader();
		var filePath = _view.GetFilePath();

		try
		{
			var employees = _csvService.ImportFromCsv(filePath);

			foreach (var employee in employees)
			{
				_repository.Create(employee);
			}

			_view.ShowSuccessMessage($"Berhasil mengimport {employees.Count} data karyawan dari CSV!");
		}
		catch (Exception ex)
		{
			_view.ShowErrorMessage($"Gagal mengimport CSV: {ex.Message}");
		}
	}

	private void ExportToCsv()
	{
		_view.ShowExportHeader();
		var filePath = _view.GetFilePath();

		try
		{
			var employees = _repository.GetAll();
			_csvService.ExportToCsv(employees, filePath);
			_view.ShowSuccessMessage($"Berhasil mengexport {employees.Count} data karyawan ke CSV!");
		}
		catch (Exception ex)
		{
			_view.ShowErrorMessage($"Gagal mengexport CSV: {ex.Message}");
		}
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				_repository?.Dispose();
			}
			_disposed = true;
		}
	}
}