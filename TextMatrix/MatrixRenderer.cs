namespace TextMatrix;

using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Menangani rendering animasi Matrix menggunakan Spectre.Console
/// </summary>
public class MatrixRenderer
{
    private readonly int _consoleWidth;
    private readonly int _consoleHeight;
    private Dictionary<string, (int y, int intensity)> _displayChars = new();

    public MatrixRenderer()
    {
        _consoleWidth = Console.WindowWidth - 1;
        _consoleHeight = Console.WindowHeight - 1;
    }

    /// <summary>
    /// Render frame animasi dengan efek Matrix
    /// </summary>
    public void Render(MatrixAnimator animator)
    {
        _displayChars.Clear();

        var columns = animator.GetColumns();
        var inputText = animator.GetInputText();
        int centerY = _consoleHeight / 2;
        int centerX = (_consoleWidth - inputText.Length) / 2;

        // Render kolom-kolom jatuh
        foreach (var column in columns)
        {
            RenderColumn(column);
        }

        // Display buffer ke console dengan markup
        DisplayToConsole(inputText, centerX, centerY, animator.GetCurrentFrame());
    }

    /// <summary>
    /// Render satu kolom dengan efek trail dan simpan ke dictionary
    /// </summary>
    private void RenderColumn(MatrixColumn column)
    {
        for (int i = 0; i < column.TrailLength; i++)
        {
            int y = column.Y - i;

            if (y >= 0 && y < _consoleHeight && column.X < _consoleWidth)
            {
                string key = $"{column.X}_{y}";
                int intensity = (int)(100 * (1 - (float)i / column.TrailLength));

                // Simpan karakter dengan intensitas tertinggi jika ada konflik
                if (!_displayChars.ContainsKey(key) || intensity > _displayChars[key].intensity)
                {
                    _displayChars[key] = (y, intensity);
                }
            }
        }
    }

    /// <summary>
    /// Tampilkan frame ke console
    /// </summary>
    private void DisplayToConsole(string inputText, int centerX, int centerY, int frame)
    {
        for (int y = 0; y < _consoleHeight; y++)
        {
            var lineBuilder = new StringBuilder();

            for (int x = 0; x < _consoleWidth; x++)
            {
                string key = $"{x}_{y}";
                char c = ' ';
                bool isInputText = y == centerY && x >= centerX && x < centerX + inputText.Length;

                // Cek apakah ada karakter dari kolom jatuh di posisi ini
                if (_displayChars.ContainsKey(key))
                {
                    var (_, intensity) = _displayChars[key];
                    // Ambil karakter pertama dari input atau gunakan karakter jatuh
                    if (isInputText)
                    {
                        c = inputText[x - centerX];
                        bool isGlowing = (frame + (x - centerX)) % 6 < 3;
                        string color = isGlowing ? "white on darkgreen" : "lime";
                        lineBuilder.Append($"[{color}]{EscapeMarkup(c.ToString())}[/]");
                    }
                    else
                    {
                        c = '█';  // Block character untuk visual yang lebih baik
                        // Determine color berdasarkan intensity
                        string color = intensity > 66 ? "lime" : (intensity > 33 ? "green" : "darkgreen");
                        lineBuilder.Append($"[{color}]{EscapeMarkup(c.ToString())}[/]");
                    }
                }
                else if (isInputText)
                {
                    c = inputText[x - centerX];
                    bool isGlowing = (frame + (x - centerX)) % 6 < 3;
                    string color = isGlowing ? "white on darkgreen" : "lime";
                    lineBuilder.Append($"[{color}]{EscapeMarkup(c.ToString())}[/]");
                }
                else
                {
                    lineBuilder.Append(' ');
                }
            }

            AnsiConsole.MarkupLine(lineBuilder.ToString());
        }
    }

    /// <summary>
    /// Escape special characters untuk Spectre.Console markup
    /// </summary>
    private string EscapeMarkup(string text)
    {
        return text.Replace("[", "[[").Replace("]", "]]");
    }

    /// <summary>
    /// Clear screen dan reset
    /// </summary>
    public void Clear()
    {
        AnsiConsole.Clear();
    }
}

