using CliAppMySqlCrud.Models;
using Spectre.Console;

namespace CliAppMySqlCrud.Views;

public class ConsoleView
{
    public void ShowTitle(string title)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText(title)
                .Centered()
                .Color(Color.Blue));
        AnsiConsole.WriteLine();
    }

    public void ShowMenu()
    {
        var menu = new Panel(
            new Markup("[bold yellow]MENU UTAMA[/]\n\n" +
                       "[cyan]1.[/] Lihat Semua Data Karyawan\n" +
                       "[cyan]2.[/] Tambah Karyawan\n" +
                       "[cyan]3.[/] Update Karyawan\n" +
                       "[cyan]4.[/] Hapus Karyawan\n" +
                       "[cyan]5.[/] Cari Karyawan (Filter)\n" +
                       "[cyan]6.[/] Import dari CSV\n" +
                       "[cyan]7.[/] Export ke CSV\n" +
                       "[cyan]0.[/] Keluar"))
        {
            Border = BoxBorder.Double,
            Padding = new Padding(2, 1)
        };

        AnsiConsole.Write(menu);
        AnsiConsole.WriteLine();
    }

    public void ShowFilterMenu()
    {
        var menu = new Panel(
            new Markup("[bold yellow]FILTER DATA KARYAWAN[/]\n\n" +
                       "[cyan]1.[/] Filter berdasarkan ID\n" +
                       "[cyan]2.[/] Filter berdasarkan Nama\n" +
                       "[cyan]3.[/] Filter berdasarkan Salary\n" +
                       "[cyan]4.[/] Filter berdasarkan Status\n" +
                       "[cyan]0.[/] Kembali ke Menu Utama"))
        {
            Border = BoxBorder.Double,
            Padding = new Padding(2, 1)
        };

        AnsiConsole.Write(menu);
        AnsiConsole.WriteLine();
    }

    public void DisplayEmployees(List<Employee> employees, string title = "DAFTAR KARYAWAN")
    {
        if (employees.Count == 0)
        {
            ShowWarning("Tidak ada data karyawan");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Blue)
            .Title($"[bold yellow]{title}[/]");

        table.AddColumn(new TableColumn("[bold cyan]Row ID[/]").Centered());
        table.AddColumn(new TableColumn("[bold cyan]ID[/]").Centered());
        table.AddColumn(new TableColumn("[bold cyan]Name[/]"));
        table.AddColumn(new TableColumn("[bold cyan]Salary[/]").RightAligned());
        table.AddColumn(new TableColumn("[bold cyan]Status[/]").Centered());

        foreach (var emp in employees)
        {
            string statusDisplay = emp.Status ? "[green]Active (1)[/]" : "[red]Inactive (0)[/]";
            string salaryFormatted = emp.Salary.ToString("N2");

            table.AddRow(
                emp.RowId.ToString(),
                emp.Id,
                emp.Name,
                salaryFormatted,
                statusDisplay
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    public void ShowSuccess(string message)
    {
        AnsiConsole.MarkupLine($"[green]✓ {message}[/]");
        AnsiConsole.WriteLine();
    }

    public void ShowError(string message)
    {
        AnsiConsole.MarkupLine($"[red]✗ {message}[/]");
        AnsiConsole.WriteLine();
    }

    public void ShowWarning(string message)
    {
        AnsiConsole.MarkupLine($"[yellow]⚠ {message}[/]");
        AnsiConsole.WriteLine();
    }

    public void ShowInfo(string message)
    {
        AnsiConsole.MarkupLine($"[blue]ℹ {message}[/]");
        AnsiConsole.WriteLine();
    }

    public string GetInput(string prompt)
    {
        return AnsiConsole.Ask<string>($"[cyan]{prompt}:[/]");
    }

    public string GetOptionalInput(string prompt, string currentValue = "")
    {
        string displayValue = string.IsNullOrEmpty(currentValue) ? "" : $" (saat ini: {currentValue})";
        AnsiConsole.MarkupLine($"[cyan]{prompt}{displayValue}[/]");
        AnsiConsole.MarkupLine("[grey](Tekan Enter untuk melewati)[/]");
        return Console.ReadLine() ?? string.Empty;
    }

    public decimal GetDecimalInput(string prompt)
    {
        while (true)
        {
            try
            {
                string input = AnsiConsole.Ask<string>($"[cyan]{prompt}:[/]");
                return decimal.Parse(input);
            }
            catch
            {
                ShowError("Input tidak valid. Masukkan angka yang benar.");
            }
        }
    }

    public decimal GetOptionalDecimalInput(string prompt, decimal currentValue)
    {
        AnsiConsole.MarkupLine($"[cyan]{prompt} (saat ini: {currentValue})[/]");
        AnsiConsole.MarkupLine("[grey](Tekan Enter untuk melewati)[/]");
        string input = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(input))
        {
            return currentValue;
        }

        try
        {
            return decimal.Parse(input);
        }
        catch
        {
            ShowError("Input tidak valid. Menggunakan nilai sebelumnya.");
            return currentValue;
        }
    }

    public bool GetBoolInput(string prompt)
    {
        while (true)
        {
            string input = AnsiConsole.Ask<string>($"[cyan]{prompt} (1=Yes/0=No):[/]");
            if (input == "1" || input.ToLower() == "yes" || input.ToLower() == "y")
                return true;
            if (input == "0" || input.ToLower() == "no" || input.ToLower() == "n")
                return false;

            ShowError("Input tidak valid. Masukkan 1/Yes atau 0/No.");
        }
    }

    public bool GetOptionalBoolInput(string prompt, bool currentValue)
    {
        string currentDisplay = currentValue ? "Active (1)" : "Inactive (0)";
        AnsiConsole.MarkupLine($"[cyan]{prompt} (saat ini: {currentDisplay})[/]");
        AnsiConsole.MarkupLine("[grey](Tekan Enter untuk melewati)[/]");
        string input = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(input))
        {
            return currentValue;
        }

        if (input == "1" || input.ToLower() == "yes" || input.ToLower() == "y")
            return true;
        if (input == "0" || input.ToLower() == "no" || input.ToLower() == "n")
            return false;

        ShowError("Input tidak valid. Menggunakan nilai sebelumnya.");
        return currentValue;
    }

    public bool Confirm(string message)
    {
        return AnsiConsole.Confirm($"[yellow]{message}[/]");
    }

    public void WaitForKeyPress()
    {
        AnsiConsole.MarkupLine("[grey]Tekan tombol apapun untuk melanjutkan...[/]");
        Console.ReadKey(true);
    }
}