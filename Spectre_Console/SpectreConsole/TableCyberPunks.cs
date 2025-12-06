using Spectre.Console;

namespace SpectreConsole;

public class TableCyberPunks
{
    public void CreateTable()
    {
        // Separator header bergaya futuristik
        var header = new Rule("[cyan]CYBER UI[/]");
        header.Justification = Justify.Center;           // alignment yang benar
        header.Style = new Style(Color.Cyan);
        AnsiConsole.Write(header);                       // gunakan Write, bukan Render

        // Tabel
        var table = new Table()
            .Border(TableBorder.Rounded)                 // border modern
            .BorderColor(Color.Cyan);

        table.Title("[bold cyan]🚀 Data Matrix[/]");
        table.Centered();
        table.ShowRowSeparators = true;

        // Kolom dengan padding
        var colId = new TableColumn("[bold white]ID[/]").Centered();
        colId.Padding = new Padding(1, 0, 1, 0);

        var colNama = new TableColumn("[bold cyan]Nama[/]").LeftAligned();
        colNama.Padding = new Padding(2, 0, 2, 0);

        var colRole = new TableColumn("[bold magenta]Role[/]").RightAligned();
        colRole.Padding = new Padding(2, 0, 2, 0);

        table.AddColumn(colId);
        table.AddColumn(colNama);
        table.AddColumn(colRole);

        // Sel dengan foreground/background neon
        table.AddRow(
            "[black on #00ff9d] 001 [/]",
            "[black on #f5d300] Rangga [/]",
            "[black on #56b4ff] Developer [/]"
        );
        table.AddRow(
            "[black on #00ff9d] 002 [/]",
            "[black on #f5d300] Alex [/]",
            "[black on #ff70ff] Designer [/]"
        );
        table.AddRow(
            "[black on #00ff9d] 003 [/]",
            "[black on #f5d300] Mika [/]",
            "[black on #56b4ff] Tester [/]"
        );

        // Padding luar menggunakan Padder
        var padded = new Padder(table)
        {
            Padding = new Padding(left: 2, top: 0, right: 2, bottom: 0)
        };

        AnsiConsole.Write(padded);

        // Footer tip
        var footer = new Rule("[grey]▼[/]");
        footer.Justification = Justify.Center;
        footer.Style = new Style(Color.Grey);
        AnsiConsole.Write(footer);

        AnsiConsole.MarkupLine("[grey]Gunakan font monospace: Cascadia Code / Fira Code / JetBrains Mono[/]");
    }
}
