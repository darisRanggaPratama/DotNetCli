using Spectre.Console;

namespace AnimasiMatrix;

class Program
    {
        static async Task Main(string[] args)
        {
            Console.CursorVisible = false;
            
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new FigletText("MATRIX EFFECT")
                    .Centered()
                    .Color(Color.Green));
            
            AnsiConsole.WriteLine();
            var userText = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Masukkan teks untuk animasi Matrix:[/]")
                    .PromptStyle("green"));
            
            AnsiConsole.Clear();
            
            await RunMatrixAnimation(userText);
            
            Console.CursorVisible = true;
            AnsiConsole.MarkupLine("\n[green]Tekan tombol apapun untuk keluar...[/]");
            Console.ReadKey();
        }
        
        static async Task RunMatrixAnimation(string text)
        {
            var width = Console.WindowWidth;
            var height = Console.WindowHeight - 2;
            var random = new Random();
            var columns = new List<MatrixColumn>();
            
            // Inisialisasi kolom matrix
            for (int i = 0; i < width; i++)
            {
                columns.Add(new MatrixColumn(i, height, random));
            }
            
            var userTextChars = text.ToCharArray();
            var textPosition = 0;
            var textStartColumn = (width - text.Length) / 2;
            var textRow = height / 2;
            var textRevealed = false;
            
            var canvas = new Canvas(width, height);
            
            // Animasi utama
            for (int frame = 0; frame < 300; frame++)
            {
                canvas = new Canvas(width, height);
                
                // Update dan render setiap kolom
                foreach (var column in columns)
                {
                    column.Update();
                    column.Render(canvas);
                }
                
                // Tampilkan teks user secara bertahap
                if (frame > 50 && !textRevealed)
                {
                    for (int i = 0; i < Math.Min(textPosition, userTextChars.Length); i++)
                    {
                        if (textStartColumn + i >= 0 && textStartColumn + i < width && textRow >= 0 && textRow < height)
                        {
                            canvas.SetPixel(textStartColumn + i, textRow, Color.White);
                        }
                    }
                    
                    if (frame % 2 == 0 && textPosition < userTextChars.Length)
                    {
                        textPosition++;
                    }
                    
                    if (textPosition >= userTextChars.Length)
                    {
                        textRevealed = true;
                    }
                }
                else if (textRevealed)
                {
                    // Tampilkan teks lengkap dengan warna putih terang
                    for (int i = 0; i < userTextChars.Length; i++)
                    {
                        if (textStartColumn + i >= 0 && textStartColumn + i < width && textRow >= 0 && textRow < height)
                        {
                            canvas.SetPixel(textStartColumn + i, textRow, Color.White);
                        }
                    }
                }
                
                AnsiConsole.Cursor.SetPosition(0, 0);
                AnsiConsole.Write(canvas);
                
                // Render teks sebagai text biasa di tengah
                if (textPosition > 0)
                {
                    Console.SetCursorPosition(textStartColumn, textRow);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(new string(userTextChars, 0, textPosition));
                    Console.ResetColor();
                }
                
                await Task.Delay(50);
            }
        }
    }