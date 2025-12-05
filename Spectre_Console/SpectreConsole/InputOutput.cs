using Spectre.Console;

namespace SpectreConsole
{
    public class InputOutput
    {
        public void ShowInputOutput()
        {                
            string name = AnsiConsole.Ask<string>("What is your [blue]name[/]?");
            AnsiConsole.MarkupLine($"My name is [yellow]{name}[/]");

            string job = AnsiConsole.Prompt(
                new TextPrompt<string>("Are you work in office?")
                .AddChoice("yes")
                .AddChoice("no")
                .DefaultValue("no")
            );
            AnsiConsole.MarkupLine($"[blue]{job}[/] I have");

            int yourAge = AnsiConsole.Prompt(
                new TextPrompt<int>("What is your age?")
                .Validate((x) => x switch
                {
                    < 1 => ValidationResult.Error("You were not born yet"),
                    > 120 => ValidationResult.Error("You are too old"),
                    _ => ValidationResult.Success()
                })
            );

            int age = AnsiConsole.Ask<int>("Enter your [green]age[/]: ");
            AnsiConsole.MarkupLine($"You entered age: [bold blue]{age}[/]");

            bool isHappy = AnsiConsole.Confirm("Are you [yellow]happy[/]?");
            AnsiConsole.MarkupLine($"Happy: [bold green]{isHappy}[/]");

            bool isHuman = AnsiConsole.Ask<bool>("Are you a [red]human[/]? (true/false)");
            AnsiConsole.MarkupLine($"Are you a human? [bold red]{isHuman}[/]");

            Console.ReadLine();
            AnsiConsole.Clear();
        }
    }
}