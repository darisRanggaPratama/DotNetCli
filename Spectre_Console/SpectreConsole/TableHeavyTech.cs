using Spectre.Console;

namespace SpectreConsole;

public class TableHeavyTech
{
    public void CreateTable()
    {
        var table = new Table();
        
        // Border Heavy Tech
        table.Border = TableBorder.Square;
        table.BorderColor(Color.Blue);
        table.Title("[bold blue]Tech Blueprint[/]"); 

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