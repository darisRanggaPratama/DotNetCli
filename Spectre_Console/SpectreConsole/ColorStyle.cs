using Spectre.Console;

namespace SpectreConsole
{
    public class ColorStyle
    {
        public void ColorAndStyle()
        {
            AnsiConsole.MarkupLine("[blue]It is inline markup\n[/]");
            AnsiConsole.MarkupLine("[white on red]Highlight text\n[/]");
            AnsiConsole.MarkupLine("[red bold on white]bold & Highlight text\n[/]");

            Style danger = new(
                foreground: Color.Blue,
                background: Color.White,
                decoration: Decoration.Italic | Decoration.Bold | Decoration.Underline | Decoration.SlowBlink               
            );

            AnsiConsole.Write(new Markup("Danger information", danger));
            AnsiConsole.MarkupLine("[bold red]more information...[/]");

            Console.ReadLine();
            AnsiConsole.Clear();
        }
    }
}