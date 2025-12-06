using Spectre.Console;

namespace SpectreConsole;

class Multiselect
{
    public void MultiSelection()
    {
        List<string> names =
        [
            "Eren Yeager",
            "Mikasa Ackerman",
            "Armin Arlert",
            "Frieren",
            "Fern",
            "Saitama",
            "Levi Ackerman",
            "Kamado Tanjirou"
        ];

        List<string> family =
        [
            "Ackerman",
            "Charity",
            "Christopher"
        ];
        
        List<string> favorites = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
            .Title("Which [green]characters[/] do you like?")
            // .PageSize(6)
            .MoreChoicesText("[grey](Move up and down to reveal more characters)[/]")
            .InstructionsText(
                "[grey](Press [blue]<space>[/] to toggle a character, " +
                "[green]<enter>[/] to accept)[/]"
            )
            // .AddChoices(names)
            .AddChoiceGroup("Common name:", names)
            .AddChoiceGroup("Family:", family)
        );

        foreach (string name in favorites)
        {
            Console.WriteLine(name);
        }

        Console.ReadLine();
        AnsiConsole.Clear();
    }
}