using Spectre.Console;
using TextMatrix;

// Setup console
Console.OutputEncoding = System.Text.Encoding.UTF8;
AnsiConsole.Clear();

// Welcome message
AnsiConsole.MarkupLine("[green bold]╔════════════════════════════════════╗[/]");
AnsiConsole.MarkupLine("[green bold]║   Matrix Text Animation Console    ║[/]");
AnsiConsole.MarkupLine("[green bold]╚════════════════════════════════════╝[/]");
AnsiConsole.MarkupLine(" ");

// Default configuration
var config = new AnimationConfig();

// Main loop
while (true)
{
    // Show menu
    AnsiConsole.MarkupLine("[lime]═══════════════════════════════════[/]");
    AnsiConsole.MarkupLine("[lime]1. Animate Text (Quick)[/]");
    AnsiConsole.MarkupLine("[lime]2. Settings[/]");
    AnsiConsole.MarkupLine("[lime]3. Exit[/]");
    AnsiConsole.MarkupLine("[lime]═══════════════════════════════════[/]");
    AnsiConsole.MarkupLine("[lime]Select option (1-3):[/]");
    
    string? menuChoice = Console.ReadLine();

    if (menuChoice == "1")
    {
        AnsiConsole.MarkupLine("[lime]Enter text to animate:[/]");
        string? userInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(userInput))
        {
            AnsiConsole.MarkupLine("[yellow]Text cannot be empty![/]");
            System.Threading.Thread.Sleep(1000);
            AnsiConsole.Clear();
            continue;
        }

        RunAnimation(userInput, config);
        AnsiConsole.Clear();
    }
    else if (menuChoice == "2")
    {
        ShowSettings(config);
        AnsiConsole.Clear();
    }
    else if (menuChoice == "3")
    {
        AnsiConsole.MarkupLine("[green]Goodbye![/]");
        break;
    }
    else
    {
        AnsiConsole.MarkupLine("[yellow]Invalid option! Please select 1-3[/]");
        System.Threading.Thread.Sleep(1000);
        AnsiConsole.Clear();
    }
}

/// <summary>
/// Menjalankan animasi untuk teks yang diberikan
/// </summary>
void RunAnimation(string inputText, AnimationConfig config)
{
    int consoleWidth = Console.WindowWidth - 1;
    int consoleHeight = Console.WindowHeight - 1;

    var animator = new MatrixAnimator(consoleWidth, consoleHeight, inputText, config);
    var renderer = new MatrixRenderer();

    renderer.Clear();

    // Run animation
    var startTime = DateTime.Now;
    while ((DateTime.Now - startTime).TotalMilliseconds < config.Duration)
    {
        renderer.Clear();
        animator.Update();
        renderer.Render(animator);

        System.Threading.Thread.Sleep(config.FrameDelay);

        // Check for early exit (ESC key)
        if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
        {
            break;
        }
    }

    AnsiConsole.MarkupLine(" ");
    AnsiConsole.MarkupLine("[green]Press any key to continue...[/]");
    Console.ReadKey(true);
}

/// <summary>
/// Menampilkan menu settings
/// </summary>
void ShowSettings(AnimationConfig config)
{
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[cyan bold]Animation Settings[/]");
    AnsiConsole.MarkupLine("[cyan]═════════════════════════════════[/]");
    AnsiConsole.MarkupLine($"[lime]Duration: {config.Duration}ms (1000-15000)[/]");
    AnsiConsole.MarkupLine($"[lime]Frame Delay: {config.FrameDelay}ms (20-100)[/]");
    AnsiConsole.MarkupLine($"[lime]Speed: {config.Speed} (1-5)[/]");
    AnsiConsole.MarkupLine($"[lime]Trail Length: {config.TrailLength} (5-20)[/]");
    AnsiConsole.MarkupLine($"[lime]Density: {config.Density} (1-5)[/]");
    AnsiConsole.MarkupLine("[cyan]═════════════════════════════════[/]");
    AnsiConsole.MarkupLine("[yellow]1. Change Duration[/]");
    AnsiConsole.MarkupLine("[yellow]2. Change Frame Delay[/]");
    AnsiConsole.MarkupLine("[yellow]3. Change Speed[/]");
    AnsiConsole.MarkupLine("[yellow]4. Change Trail Length[/]");
    AnsiConsole.MarkupLine("[yellow]5. Change Density[/]");
    AnsiConsole.MarkupLine("[yellow]6. Reset to Defaults[/]");
    AnsiConsole.MarkupLine("[yellow]7. Back to Menu[/]");
    AnsiConsole.MarkupLine("[yellow]Select option (1-7):[/]");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            AnsiConsole.MarkupLine("[lime]Enter duration (1000-15000):[/]");
            if (int.TryParse(Console.ReadLine(), out int duration) && duration >= 1000 && duration <= 15000)
            {
                config.Duration = duration;
                AnsiConsole.MarkupLine("[green]✓ Duration updated![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]✗ Invalid input![/]");
            }
            break;

        case "2":
            AnsiConsole.MarkupLine("[lime]Enter frame delay (20-100):[/]");
            if (int.TryParse(Console.ReadLine(), out int delay) && delay >= 20 && delay <= 100)
            {
                config.FrameDelay = delay;
                AnsiConsole.MarkupLine("[green]✓ Frame delay updated![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]✗ Invalid input![/]");
            }
            break;

        case "3":
            AnsiConsole.MarkupLine("[lime]Enter speed (1-5):[/]");
            if (int.TryParse(Console.ReadLine(), out int speed) && speed >= 1 && speed <= 5)
            {
                config.Speed = speed;
                AnsiConsole.MarkupLine("[green]✓ Speed updated![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]✗ Invalid input![/]");
            }
            break;

        case "4":
            AnsiConsole.MarkupLine("[lime]Enter trail length (5-20):[/]");
            if (int.TryParse(Console.ReadLine(), out int trail) && trail >= 5 && trail <= 20)
            {
                config.TrailLength = trail;
                AnsiConsole.MarkupLine("[green]✓ Trail length updated![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]✗ Invalid input![/]");
            }
            break;

        case "5":
            AnsiConsole.MarkupLine("[lime]Enter density (1-5):[/]");
            if (int.TryParse(Console.ReadLine(), out int density) && density >= 1 && density <= 5)
            {
                config.Density = density;
                AnsiConsole.MarkupLine("[green]✓ Density updated![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]✗ Invalid input![/]");
            }
            break;

        case "6":
            config = new AnimationConfig();
            AnsiConsole.MarkupLine("[green]✓ Settings reset to defaults![/]");
            break;

        case "7":
            return;

        default:
            AnsiConsole.MarkupLine("[red]Invalid option![/]");
            break;
    }

    System.Threading.Thread.Sleep(2000);
}

