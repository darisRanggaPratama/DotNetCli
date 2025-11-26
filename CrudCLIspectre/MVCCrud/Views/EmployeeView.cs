using EmployeeManagement.Models;
using Spectre.Console;
using Rule = Spectre.Console.Rule;

namespace EmployeeManagement.Views;

public class EmployeeView
{
	public string ShowMainMenu()
	{
		Console.Clear();

		var panel = new Panel(
			new FigletText("Employee Management")
				.Centered()
				.Color(Color.Cyan))
		{
			Border = BoxBorder.Double,
			BorderStyle = new Style(Color.Cyan1)
		};

		AnsiConsole.Write(panel);
		AnsiConsole.WriteLine();

		var choice = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title("[cyan1]Pilih Menu:[/]")
				.PageSize(10)
				.AddChoices(new[]
				{
					"1. Tambah Karyawan",
					"2. Lihat Semua Karyawan",
					"3. Update Karyawan",
					"4. Hapus Karyawan",
					"5. Filter/Cari Karyawan",
					"6. Import dari CSV",
					"7. Export ke CSV",
					"0. Keluar"
				}));

		return choice.Split('.')[0];
	}

	public string ShowFilterMenu()
	{
		AnsiConsole.WriteLine();
		return AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title("[yellow]Filter berdasarkan:[/]")
				.AddChoices(new[]
				{
					"1. Nama",
					"2. Gaji",
					"3. Status"
				})).Split('.')[0];
	}

	public void ShowCreateHeader()
	{
		Console.Clear();
		AnsiConsole.Write(new Rule("[green]Tambah Karyawan Baru[/]").RuleStyle("green").LeftJustified());
		AnsiConsole.WriteLine();
	}

	public void ShowUpdateHeader()
	{
		Console.Clear();
		AnsiConsole.Write(new Rule("[yellow]Update Data Karyawan[/]").RuleStyle("yellow").LeftJustified());
		AnsiConsole.WriteLine();
	}

	public void ShowDeleteHeader()
	{
		Console.Clear();
		AnsiConsole.Write(new Rule("[red]Hapus Data Karyawan[/]").RuleStyle("red").LeftJustified());
		AnsiConsole.WriteLine();
	}

	public void ShowImportHeader()
	{
		Console.Clear();
		AnsiConsole.Write(new Rule("[cyan]Import Data dari CSV[/]").RuleStyle("cyan").LeftJustified());
		AnsiConsole.WriteLine();
	}

	public void ShowExportHeader()
	{
		Console.Clear();
		AnsiConsole.Write(new Rule("[cyan]Export Data ke CSV[/]").RuleStyle("cyan").LeftJustified());
		AnsiConsole.WriteLine();
	}

	public Employee GetEmployeeInput()
	{
		var name = AnsiConsole.Ask<string>("[cyan]Nama:[/]");
		var salary = AnsiConsole.Ask<decimal>("[cyan]Gaji:[/]");
		var status = AnsiConsole.Confirm("[cyan]Status aktif?[/]");

		return new Employee
		{
			Name = name,
			Salary = salary,
			Status = status
		};
	}

	public int GetEmployeeId()
	{
		return AnsiConsole.Ask<int>("[cyan]Masukkan ID Karyawan:[/]");
	}

	public string GetSearchName()
	{
		return AnsiConsole.Ask<string>("[cyan]Masukkan nama yang dicari:[/]");
	}

	public (decimal min, decimal max) GetSalaryRange()
	{
		var min = AnsiConsole.Ask<decimal>("[cyan]Gaji minimum:[/]");
		var max = AnsiConsole.Ask<decimal>("[cyan]Gaji maksimum:[/]");
		return (min, max);
	}

	public bool GetStatusFilter()
	{
		return AnsiConsole.Confirm("[cyan]Cari status aktif (Yes)?[/]");
	}

	public string GetFilePath()
	{
		return AnsiConsole.Ask<string>("[cyan]Masukkan path file:[/]");
	}

	public bool ConfirmDelete()
	{
		return AnsiConsole.Confirm("[red]Apakah Anda yakin ingin menghapus data ini?[/]");
	}

	public void DisplayEmployees(List<Employee> employees, string title)
	{
		Console.Clear();
		AnsiConsole.Write(new Rule($"[cyan1]{title}[/]").RuleStyle("cyan1").LeftJustified());
		AnsiConsole.WriteLine();

		if (employees.Count == 0)
		{
			AnsiConsole.MarkupLine("[yellow]Tidak ada data karyawan.[/]");
			return;
		}

		var table = new Table()
			.Border(TableBorder.Rounded)
			.BorderColor(Color.Cyan1);

		table.AddColumn(new TableColumn("[cyan1]ID[/]").Centered());
		table.AddColumn(new TableColumn("[cyan1]Nama[/]").LeftAligned());
		table.AddColumn(new TableColumn("[cyan1]Gaji[/]").RightAligned());
		table.AddColumn(new TableColumn("[cyan1]Status[/]").Centered());

		foreach (var emp in employees)
		{
			var statusColor = emp.Status ? "green" : "red";
			table.AddRow(
				emp.Id.ToString(),
				emp.Name,
				emp.Salary.ToString("N0"),
				$"[{statusColor}]{emp.StatusDisplay}[/]"
			);
		}

		AnsiConsole.Write(table);
		AnsiConsole.WriteLine();
		AnsiConsole.MarkupLine($"[dim]Total: {employees.Count} karyawan[/]");
	}

	public void DisplaySingleEmployee(Employee employee)
	{
		var table = new Table()
			.Border(TableBorder.Rounded)
			.BorderColor(Color.Yellow);

		table.AddColumn("[yellow]Field[/]");
		table.AddColumn("[yellow]Value[/]");

		table.AddRow("ID", employee.Id.ToString());
		table.AddRow("Nama", employee.Name);
		table.AddRow("Gaji", employee.Salary.ToString("N0"));
		table.AddRow("Status", employee.StatusDisplay);

		AnsiConsole.Write(table);
		AnsiConsole.WriteLine();
	}

	public void ShowSuccessMessage(string message)
	{
		AnsiConsole.MarkupLine($"[green]✓ {message}[/]");
	}

	public void ShowErrorMessage(string message)
	{
		AnsiConsole.MarkupLine($"[red]✗ {message}[/]");
	}

	public void ShowInfoMessage(string message)
	{
		AnsiConsole.MarkupLine($"[blue]ℹ {message}[/]");
	}

	public void ShowExitMessage()
	{
		Console.Clear();
		AnsiConsole.Write(
			new FigletText("Terima Kasih!")
				.Centered()
				.Color(Color.Green));
		AnsiConsole.MarkupLine("\n[dim]Aplikasi ditutup.[/]");
	}

	public void PressAnyKey()
	{
		AnsiConsole.WriteLine();
		AnsiConsole.MarkupLine("[dim]Tekan sembarang tombol untuk melanjutkan...[/]");
		Console.ReadKey(true);
	}
}