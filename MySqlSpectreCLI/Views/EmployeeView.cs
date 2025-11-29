using MySqlSpectreCLI.Models;
using Spectre.Console;

namespace MySqlSpectreCLI.Views
{

	public class EmployeeView
	{
		public void ShowMenu()
		{
			AnsiConsole.Clear();
			var panel = new Panel("[bold yellow]Employee Management System[/]")
			{
				Border = BoxBorder.Double,
				Padding = new Padding(2, 1)
			};
			AnsiConsole.Write(panel);

			AnsiConsole.WriteLine();
		}

		public string GetMenuChoice()
		{
			return AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("[green]Pilih menu:[/]")
					.PageSize(10)
					.AddChoices(new[]
					{
						"1. Lihat semua karyawan",
						"2. Tambah karyawan",
						"3. Edit karyawan",
						"4. Hapus karyawan",
						"5. Cari karyawan",
						"6. Upload CSV",
						"7. Download CSV",
						"8. Keluar"
					}));
		}

		public void DisplayEmployees(List<Employee> employees)
		{
			if (employees.Count == 0)
			{
				AnsiConsole.MarkupLine("[yellow]Tidak ada data karyawan.[/]");
				return;
			}

			var table = new Table();
			table.Border(TableBorder.Rounded);
			table.AddColumn("[bold]Row ID[/]");
			table.AddColumn("[bold]ID Karyawan[/]");
			table.AddColumn("[bold]Nama[/]");
			table.AddColumn("[bold]Gaji[/]");
			table.AddColumn("[bold]Status[/]");

			foreach (var employee in employees)
			{
				table.AddRow(
					employee.RowId.ToString(),
					employee.Id,
					employee.Name,
					$"Rp {employee.Salary:N0}",
					employee.Status ? "[green]Active[/]" : "[red]Inactive[/]"
				);
			}

			AnsiConsole.Write(table);
		}

		public Employee GetEmployeeInput()
		{
			var employee = new Employee();

			employee.Id = AnsiConsole.Ask<string>("[cyan]ID Karyawan (max 6 digit):[/]");
			while (employee.Id.Length > 6)
			{
				AnsiConsole.MarkupLine("[red]ID tidak boleh lebih dari 6 digit![/]");
				employee.Id = AnsiConsole.Ask<string>("[cyan]ID Karyawan (max 6 digit):[/]");
			}

			employee.Name = AnsiConsole.Ask<string>("[cyan]Nama Karyawan:[/]");
			employee.Salary = AnsiConsole.Ask<decimal>("[cyan]Gaji:[/]");
			employee.Status = AnsiConsole.Confirm("[cyan]Status aktif?[/]", true);

			return employee;
		}

		public string GetEmployeeId()
		{
			return AnsiConsole.Ask<string>("[cyan]Masukkan ID Karyawan:[/]");
		}

		public Employee GetUpdateInput(Employee existing)
		{
			AnsiConsole.MarkupLine("[yellow]Data saat ini:[/]");
			DisplayEmployees(new List<Employee> { existing });
			AnsiConsole.MarkupLine("[dim]Tekan ENTER untuk melewati field yang tidak ingin diubah[/]");
			AnsiConsole.WriteLine();

			var updated = new Employee
			{
				RowId = existing.RowId,
				Id = existing.Id,
				Name = existing.Name,
				Salary = existing.Salary,
				Status = existing.Status
			};

			var nameInput = AnsiConsole.Ask<string>($"[cyan]Nama[/] [dim]({existing.Name}):[/]", string.Empty);
			if (!string.IsNullOrWhiteSpace(nameInput))
			{
				updated.Name = nameInput;
			}

			var salaryInput = AnsiConsole.Ask<string>($"[cyan]Gaji[/] [dim]({existing.Salary}):[/]", string.Empty);
			if (!string.IsNullOrWhiteSpace(salaryInput) && decimal.TryParse(salaryInput, out var salary))
			{
				updated.Salary = salary;
			}

			var statusInput = AnsiConsole.Ask<string>($"[cyan]Status (1=aktif, 0=nonaktif)[/] [dim]({(existing.Status ? "1" : "0")}):[/]", string.Empty);
			if (!string.IsNullOrWhiteSpace(statusInput))
			{
				updated.Status = statusInput == "1";
			}

			return updated;
		}

		public Dictionary<string, object?> GetSearchCriteria()
		{
			var criteria = new Dictionary<string, object?>();

			AnsiConsole.MarkupLine("[yellow]Masukkan kriteria pencarian (tekan ENTER untuk melewati):[/]");

			var id = AnsiConsole.Ask<string>("[cyan]ID Karyawan:[/]", string.Empty);
			criteria["id"] = string.IsNullOrWhiteSpace(id) ? null : id;

			var name = AnsiConsole.Ask<string>("[cyan]Nama:[/]", string.Empty);
			criteria["name"] = string.IsNullOrWhiteSpace(name) ? null : name;

			var salaryInput = AnsiConsole.Ask<string>("[cyan]Gaji:[/]", string.Empty);
			criteria["salary"] = string.IsNullOrWhiteSpace(salaryInput) || !decimal.TryParse(salaryInput, out var salary) ? null : (decimal?)salary;

			var statusInput = AnsiConsole.Ask<string>("[cyan]Status (1=aktif, 0=nonaktif):[/]", string.Empty);
			criteria["status"] = string.IsNullOrWhiteSpace(statusInput) ? null : (bool?)(statusInput == "1");

			return criteria;
		}

		public string GetFilePath(string fileName)
		{
			var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			return Path.Combine(desktopPath, fileName);
		}

		public void ShowSuccess(string message)
		{
			AnsiConsole.MarkupLine($"[green]✓ {message}[/]");
		}

		public void ShowError(string message)
		{
			AnsiConsole.MarkupLine($"[red]✗ {message}[/]");
		}

		public void ShowInfo(string message)
		{
			AnsiConsole.MarkupLine($"[blue]ℹ {message}[/]");
		}

		public void WaitForKey()
		{
			AnsiConsole.WriteLine();
			AnsiConsole.MarkupLine("[dim]Tekan tombol apapun untuk melanjutkan...[/]");
			Console.ReadKey();
		}

		public bool Confirm(string message)
		{
			return AnsiConsole.Confirm(message);
		}

		public void ShowDeletePreview(Employee employee)
		{
			AnsiConsole.MarkupLine("[yellow]Data yang akan dihapus:[/]");
			DisplayEmployees(new List<Employee> { employee });
		}
	}
}