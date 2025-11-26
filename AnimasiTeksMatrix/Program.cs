using Spectre.Console;


class Program
{
    static void Main(string[] args)
    {
        AnsiConsole.Clear();
        
        AnsiConsole.Write(
            new FigletText("Matrix Text")
                .Centered()
                .Color(Color.Green));
        
        AnsiConsole.WriteLine();
        
        var inputText = AnsiConsole.Ask<string>("[green]Masukkan teks yang ingin dianimasikan:[/]");
        
        if (string.IsNullOrWhiteSpace(inputText))
        {
            AnsiConsole.MarkupLine("[red]Teks tidak boleh kosong![/]");
            return;
        }
        
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[dim]Tekan tombol apapun untuk memulai animasi...[/]");
        Console.ReadKey(true);
        
        AnsiConsole.Clear();
        AnimateMatrixText(inputText);
        
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[green]Animasi selesai! Tekan tombol apapun untuk keluar...[/]");
        Console.ReadKey(true);
    }
    
    static void AnimateMatrixText(string text)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$%^&*()_+-=[]{}|;:,.<>?";
        Random random = new Random();
        int cycles = 15;
        int delayMs = 1000;
        
        int startRow = Console.WindowHeight / 2;
        int startCol = (Console.WindowWidth - text.Length) / 2;
        
        if (startCol < 0) startCol = 0;
        if (startRow < 0) startRow = 0;
        
        char[] displayChars = new char[text.Length];
        bool[] revealed = new bool[text.Length];
        
        for (int i = 0; i < text.Length; i++)
        {
            displayChars[i] = chars[random.Next(chars.Length)];
            revealed[i] = false;
        }
        
        for (int cycle = 0; cycle < cycles; cycle++)
        {
            Console.SetCursorPosition(startCol, startRow);
            
            for (int i = 0; i < text.Length; i++)
            {
                if (!revealed[i])
                {
                    if (cycle > cycles / 2 && random.Next(100) < 20)
                    {
                        displayChars[i] = text[i];
                        revealed[i] = true;
                    }
                    else
                    {
                        displayChars[i] = chars[random.Next(chars.Length)];
                    }
                }
                
                if (revealed[i])
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(displayChars[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(displayChars[i]);
                    Console.ResetColor();
                }
            }
            
            Thread.Sleep(delayMs);
        }
        
        Console.SetCursorPosition(startCol, startRow);
        for (int i = 0; i < text.Length; i++)
        {
            displayChars[i] = text[i];
        }
        
        Console.ForegroundColor = ConsoleColor.Green;
        for (int i = 0; i < text.Length; i++)
        {
            Console.Write(displayChars[i]);
        }
        Console.ResetColor();
        
        AnsiConsole.WriteLine();
        
        for (int row = startRow + 2; row < startRow + 10 && row < Console.WindowHeight - 1; row++)
        {
            Console.SetCursorPosition(random.Next(Console.WindowWidth - 1), row);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(chars[random.Next(chars.Length)]);
            Console.ResetColor();
            Thread.Sleep(30);
        }
    }
}