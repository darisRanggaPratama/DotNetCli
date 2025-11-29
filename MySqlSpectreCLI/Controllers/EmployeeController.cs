using MySqlSpectreCLI.Repositories;
using MySqlSpectreCLI.Services;
using MySqlSpectreCLI.Views;

namespace MySqlSpectreCLI.Controllers
{
	public class EmployeeController
	{
		private readonly EmployeeRepository _repository;
		private readonly EmployeeView _view;
		private readonly CsvService _csvService;

		public EmployeeController()
		{
			_repository = new EmployeeRepository();
			_view = new EmployeeView();
			_csvService = new CsvService();
		}

		public void Run()
		{
			bool running = true;

			while (running)
			{
				_view.ShowMenu();
				var choice = _view.GetMenuChoice();

				switch (choice.Substring(0, 1))
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
						SearchEmployees();
						break;
					case "6":
						UploadCsv();
						break;
					case "7":
						DownloadCsv();
						break;
					case "8":
						running = false;
						_view.ShowInfo("Terima kasih telah menggunakan aplikasi ini!");
						break;
				}

				if (running && choice.Substring(0, 1) != "8")
				{
					_view.WaitForKey();
				}
			}
		}

		private void ViewAllEmployees()
		{
			var employees = _repository.GetAll();
			_view.DisplayEmployees(employees);
		}

		private void CreateEmployee()
		{
			var employee = _view.GetEmployeeInput();

			// Cek apakah ID sudah ada
			var existing = _repository.GetById(employee.Id);
			if (existing != null)
			{
				_view.ShowError($"ID Karyawan {employee.Id} sudah terdaftar!");
				return;
			}

			if (_repository.Create(employee))
			{
				_view.ShowSuccess("Karyawan berhasil ditambahkan!");
			}
			else
			{
				_view.ShowError("Gagal menambahkan karyawan!");
			}
		}

		private void UpdateEmployee()
		{
			var id = _view.GetEmployeeId();
			var existing = _repository.GetById(id);

			if (existing == null)
			{
				_view.ShowError($"Karyawan dengan ID {id} tidak ditemukan!");
				return;
			}

			var updated = _view.GetUpdateInput(existing);

			if (_repository.Update(updated))
			{
				_view.ShowSuccess("Data karyawan berhasil diupdate!");
			}
			else
			{
				_view.ShowError("Gagal mengupdate data karyawan!");
			}
		}

		private void DeleteEmployee()
		{
			var id = _view.GetEmployeeId();
			var existing = _repository.GetById(id);

			if (existing == null)
			{
				_view.ShowError($"Karyawan dengan ID {id} tidak ditemukan!");
				return;
			}

			_view.ShowInfo($"Data yang akan dihapus: {existing}");
			_view.ShowDeletePreview(existing);
			
			if (_view.Confirm("Apakah Anda yakin ingin menghapus data ini?"))
			{
				if (_repository.Delete(id))
				{
					_view.ShowSuccess("Data karyawan berhasil dihapus!");
				}
				else
				{
					_view.ShowError("Gagal menghapus data karyawan!");
				}
			}
			else
			{
				_view.ShowInfo("Penghapusan dibatalkan.");
			}
		}

		private void SearchEmployees()
		{
			var criteria = _view.GetSearchCriteria();

			var employees = _repository.Search(
				criteria["id"] as string,
				criteria["name"] as string,
				criteria["salary"] as decimal?,
				criteria["status"] as bool?
			);

			_view.ShowInfo($"Ditemukan {employees.Count} data karyawan:");
			_view.DisplayEmployees(employees);
		}

		private void UploadCsv()
		{
			var fileName = "employees.csv";
			var filePath = _view.GetFilePath(fileName);

			try
			{
				_view.ShowInfo($"Membaca file dari: {filePath}");
				var employees = _csvService.ReadFromCsv(filePath);

				if (employees.Count == 0)
				{
					_view.ShowError("Tidak ada data valid dalam file CSV!");
					return;
				}

				_view.ShowInfo($"Ditemukan {employees.Count} data karyawan dalam file.");

				if (_view.Confirm("Lanjutkan import data?"))
				{
					int successCount = 0;
					int failCount = 0;

					foreach (var employee in employees)
					{
						// Cek apakah ID sudah ada
						var existing = _repository.GetById(employee.Id);
						if (existing == null)
						{
							if (_repository.Create(employee))
							{
								successCount++;
							}
							else
							{
								failCount++;
							}
						}
						else
						{
							failCount++;
						}
					}

					_view.ShowSuccess($"Import selesai! Berhasil: {successCount}, Gagal: {failCount}");
				}
				else
				{
					_view.ShowInfo("Import dibatalkan.");
				}
			}
			catch (FileNotFoundException ex)
			{
				_view.ShowError(ex.Message);
			}
			catch (Exception ex)
			{
				_view.ShowError($"Terjadi kesalahan: {ex.Message}");
			}
		}

		private void DownloadCsv()
		{
			var fileName = "employees_export.csv";
			var filePath = _view.GetFilePath(fileName);

			try
			{
				var employees = _repository.GetAll();

				if (employees.Count == 0)
				{
					_view.ShowError("Tidak ada data untuk diexport!");
					return;
				}

				_csvService.WriteToCsv(filePath, employees);
				_view.ShowSuccess($"Data berhasil diexport ke: {filePath}");
				_view.ShowInfo($"Total {employees.Count} data karyawan telah diexport.");
			}
			catch (Exception ex)
			{
				_view.ShowError($"Terjadi kesalahan: {ex.Message}");
			}
		}
	}
}
