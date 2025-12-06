using Spectre.Console;

namespace SpectreConsole;

public class TableCreated
{
    public void CreateTable()
    {
        Table table = new();
        table.Centered();
        table.BorderStyle(new Style(foreground: Color.Yellow));
        table.Border(TableBorder.Rounded);
        table.ShowRowSeparators();
        
        table.AddColumn("Name");
        table.AddColumn("Age");
        table.AddColumn("City");
        
        table.Columns[0].PadLeft(5).PadRight(5);
        table.Columns[0].Centered();
        table.Columns[1].Width(15);
        table.Columns[2].RightAligned();

        table.AddRow("Eren Yeager", "19", "Shiganshina");
        table.AddRow("Mikasa Ackerman", "20", "Shiganshina");
        table.AddRow("Armin Arlert", "19", "Shiganshina");
        table.AddRow("Frieren", "Unknown", "Kingdom of Heavens");
        table.AddRow("Fern", "Unknown", "Kingdom of Heavens");
        table.AddRow("Saitama", "25", "City Z");

        AnsiConsole.Write(table);

        Console.ReadLine();
        AnsiConsole.Clear();
    }
}