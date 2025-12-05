using Spectre.Console;

namespace SpectreConsole
{
    public class ItemSelect
    {
        public void ShowItemSelect()
        {
            List<string> names = [
                "Eren Yeager",
                "Mikasa Ackerman",
                "Armin Arlert",
                "Frieren",
                "Fern",
                "Saitama"
            ];

            string favorite = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Which is your favorite character?")
                .PageSize(4)
                .MoreChoicesText("[purple](More choices)[/]")
                .AddChoices(names)
            );

            AnsiConsole.MarkupLine($"Your favorite character is [yellow] {favorite}[/]");

            Console.ReadLine();
            AnsiConsole.Clear();
        }
    }
}