using System.Globalization;
using Spectre.Console;
using MySqlCLIapp.Models;
using MySqlCLIapp.Repositories;
using MySqlCLIapp.Services;
using MySqlCLIapp.Views;

namespace MySqlCLIapp.Controllers
{
    public class EmployeeController
    {
        private readonly IEmployeeRepository _repo;
        private readonly EmployeeView _view;

        public EmployeeController(IEmployeeRepository repo, EmployeeView view)
        {
            _repo = repo;
            _view = view;
        }

        public void ListAll()
        {
            var data = _repo.GetAll();
            _view.RenderEmployees(data, "Semua Karyawan");
        }

        public void SearchById()
        {
            var id = AnsiConsole.Ask<string>("Masukkan ID (maks 6 digit):").Trim();
            var emp = _repo.GetById(id);
            if (emp is null)
            {
                AnsiConsole.MarkupLine("[yellow]Data tidak ditemukan[/]");
                return;
            }
            _view.RenderEmployees(new[] { emp }, $"Karyawan dengan ID {id}");
        }

        public void SearchByName()
        {
            var name = AnsiConsole.Ask<string>("Masukkan Nama (partial diperbolehkan):").Trim();
            var data = _repo.GetByName(name);
            _view.RenderEmployees(data, $"Karyawan dengan Nama mengandung '{name}'");
        }

        public void SearchBySalary()
        {
            var salaryStr = AnsiConsole.Ask<string>("Masukkan Salary (angka):").Trim();
            if (!decimal.TryParse(salaryStr, NumberStyles.Number, CultureInfo.InvariantCulture, out var salary))
            {
                AnsiConsole.MarkupLine("[red]Format salary tidak valid[/]");
                return;
            }
            var data = _repo.GetBySalary(salary);
            _view.RenderEmployees(data, $"Karyawan dengan Salary = {salary}");
        }

        public void SearchByStatus()
        {
            var statusStr = AnsiConsole.Ask<string>("Masukkan Status (1=yes, 0=no):").Trim();
            if (!int.TryParse(statusStr, out var status) || (status != 0 && status != 1))
            {
                AnsiConsole.MarkupLine("[red]Status harus 0 atau 1[/]");
                return;
            }
            var data = _repo.GetByStatus(status);
            _view.RenderEmployees(data, $"Karyawan dengan Status = {status}");
        }

        public void CreateEmployee()
        {
            var id = AskEmployeeId();
            var name = AnsiConsole.Ask<string>("Nama:").Trim();
            var salary = AskSalary();
            var status = AskStatus();

            _repo.Create(new Employee { Id = id, Name = name, Salary = salary, Status = status });
            AnsiConsole.MarkupLine("[green]Karyawan berhasil ditambahkan[/]");
        }

        public void UpdateEmployee()
        {
            var id = AskEmployeeId();
            var existing = _repo.GetById(id);
            if (existing is null)
            {
                AnsiConsole.MarkupLine("[yellow]Data dengan ID tersebut tidak ditemukan[/]");
                return;
            }

            // Tampilkan data dalam tabel sebelum edit
            _view.RenderEmployees(new[] { existing }, "Data karyawan yang akan diubah");

            AnsiConsole.MarkupLine("Tekan Enter untuk melewati kolom (tidak diubah)");

            var namePrompt = Markup.Escape($"Nama [{existing.Name}]:");
            var salaryPrompt = Markup.Escape($"Salary [{existing.Salary.ToString(CultureInfo.InvariantCulture)}]:");
            var statusPrompt = Markup.Escape($"Status (1/0) [{existing.Status}]:");

            var nameInput = AnsiConsole.Ask<string>(namePrompt, "");
            var salaryInput = AnsiConsole.Ask<string>(salaryPrompt, "");
            var statusInput = AnsiConsole.Ask<string>(statusPrompt, "");

            var name = string.IsNullOrWhiteSpace(nameInput) ? existing.Name : nameInput.Trim();

            decimal salary;
            if (string.IsNullOrWhiteSpace(salaryInput))
                salary = existing.Salary;
            else if (!decimal.TryParse(salaryInput, NumberStyles.Number, CultureInfo.InvariantCulture, out salary))
            {
                AnsiConsole.MarkupLine("[red]Format salary tidak valid[/]");
                return;
            }

            int status;
            if (string.IsNullOrWhiteSpace(statusInput))
                status = existing.Status;
            else if (!int.TryParse(statusInput, out status) || (status != 0 && status != 1))
            {
                AnsiConsole.MarkupLine("[red]Status harus 0 atau 1[/]");
                return;
            }

            var ok = _repo.Update(new Employee { Id = id, Name = name, Salary = salary, Status = status });
            AnsiConsole.MarkupLine(ok ? "[green]Update berhasil[/]" : "[yellow]Update gagal[/]");

            // Jika berhasil, tampilkan data hasil update dalam tabel
            if (ok)
            {
                var updated = _repo.GetById(id);
                if (updated is not null)
                {
                    _view.RenderEmployees(new[] { updated }, "Data karyawan setelah diubah");
                }
            }
        }

        public void DeleteEmployee()
        {
            var id = AskEmployeeId();

            // Ambil data terlebih dahulu untuk ditampilkan dalam tabel
            var existing = _repo.GetById(id);
            if (existing is null)
            {
                AnsiConsole.MarkupLine("[yellow]Data tidak ditemukan[/]");
                return;
            }

            // Tampilkan data dalam tabel sebelum konfirmasi hapus
            _view.RenderEmployees(new[] { existing }, "Data karyawan yang akan dihapus");

            var confirm = AnsiConsole.Confirm($"Hapus karyawan dengan ID {id}?");
            if (!confirm) return;
            var ok = _repo.Delete(id);
            AnsiConsole.MarkupLine(ok ? "[green]Hapus berhasil[/]" : "[yellow]Data tidak ditemukan[/]");
        }

        public void UploadCsvFromDesktop()
        {
            var fileName = AnsiConsole.Ask<string>("Nama file CSV di Desktop (misal: sample_employees.csv):").Trim();
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var path = Path.Combine(desktop, fileName);
            if (!File.Exists(path))
            {
                AnsiConsole.MarkupLine($"[red]File tidak ditemukan:[/] {path}");
                return;
            }

            var employees = CsvService.ReadEmployeesFromCsv(path);
            _repo.BulkUpsert(employees);
            AnsiConsole.MarkupLine("[green]Upload selesai[/]");
        }

        public void DownloadCsvToDesktop()
        {
            var fileName = AnsiConsole.Ask<string>("Nama file untuk disimpan di Desktop (misal: employees_export.csv):").Trim();
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var path = Path.Combine(desktop, fileName);
            var employees = _repo.GetAll();
            CsvService.WriteEmployeesToCsv(path, employees);
            AnsiConsole.MarkupLine($"[green]CSV disimpan:[/] {path}");
        }

        private static string AskEmployeeId()
        {
            while (true)
            {
                var id = AnsiConsole.Ask<string>("ID (maks 6 digit angka):").Trim();
                if (id.Length == 0 || id.Length > 6 || !id.All(char.IsDigit))
                {
                    AnsiConsole.MarkupLine("[red]ID harus angka dan maksimal 6 digit[/]");
                    continue;
                }
                return id;
            }
        }

        private static decimal AskSalary()
        {
            while (true)
            {
                var salaryStr = AnsiConsole.Ask<string>("Salary:").Trim();
                if (decimal.TryParse(salaryStr, NumberStyles.Number, CultureInfo.InvariantCulture, out var salary))
                    return salary;
                AnsiConsole.MarkupLine("[red]Format salary tidak valid[/]");
            }
        }

        private static int AskStatus()
        {
            while (true)
            {
                var statusStr = AnsiConsole.Ask<string>("Status (1=yes, 0=no):").Trim();
                if (int.TryParse(statusStr, out var status) && (status == 0 || status == 1))
                    return status;
                AnsiConsole.MarkupLine("[red]Status harus 0 atau 1[/]");
            }
        }
    }
}