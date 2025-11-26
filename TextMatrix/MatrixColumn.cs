namespace TextMatrix;

/// <summary>
/// Merepresentasikan satu kolom karakter yang jatuh dalam animasi Matrix
/// </summary>
public class MatrixColumn
{
    public int X { get; set; }
    public int Y { get; set; }
    public char[] Characters { get; set; }
    public int CurrentIndex { get; set; }
    public int Speed { get; set; }
    public int TrailLength { get; set; }
    public bool IsActive { get; set; }

    public MatrixColumn(int x, int y, char[] characters, int speed = 1, int trailLength = 8)
    {
        X = x;
        Y = y;
        Characters = characters;
        Speed = speed;
        TrailLength = trailLength;
        CurrentIndex = 0;
        IsActive = true;
    }

    /// <summary>
    /// Update posisi kolom berdasarkan speed
    /// </summary>
    public void Update()
    {
        Y += Speed;
        CurrentIndex = (CurrentIndex + 1) % Characters.Length;
    }

    /// <summary>
    /// Mendapatkan karakter saat ini
    /// </summary>
    public char GetCurrentChar()
    {
        return Characters[CurrentIndex];
    }
}

