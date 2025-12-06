using Spectre.Console;

namespace SpectreConsole;

public class TableFuturistic
{
    public void CreateTable()
    {
        var table = new Table();

        // Border futuristik
        table.Border(TableBorder.Rounded);      // sudut bulat, lebih modern
        table.BorderColor(Color.Cyan);               // warna neon cyan
        table.Title("[bold cyan]🚀 Data Matrix[/]"); // judul dengan emoji & warna

        // Kolom dengan style
        table.AddColumn(new TableColumn("[bold white]ID[/]").Centered());
        table.AddColumn(new TableColumn("[bold cyan]Nama[/]").LeftAligned());
        table.AddColumn(new TableColumn("[bold magenta]Role[/]").RightAligned());

        // Tambahkan baris dengan warna berbeda
        table.AddRow("[green]001[/]", "[yellow]Rangga[/]", "[blue]Developer[/]");
        table.AddRow("[green]002[/]", "[yellow]Alex[/]", "[blue]Designer[/]");
        table.AddRow("[green]003[/]", "[yellow]Mika[/]", "[blue]Tester[/]");

        // Render ke console
        AnsiConsole.Write(table);
        
        Console.ReadLine();
        AnsiConsole.Clear();
        
        
        
    }
}