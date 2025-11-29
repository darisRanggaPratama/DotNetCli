using Spectre.Console;

namespace SpectreConsole
{
    public class Basic
    {
        public void BasicText()
        {

            AnsiConsole.Write("Your Name: ");
            AnsiConsole.MarkupLine("[bold red]rangga pratama[/]");
            AnsiConsole.MarkupLine("[slowblink]Your Age: 22[/]");

            Console.ReadLine();
            AnsiConsole.Clear();
        }
    }
}