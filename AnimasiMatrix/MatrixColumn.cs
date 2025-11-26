using Spectre.Console;

namespace AnimasiMatrix;

class MatrixColumn
{
    private readonly int x;
    private readonly int maxHeight;
    private readonly Random random;
    private int y;
    private int length;
    private readonly List<char> chars;
    private int speed;

    private static readonly char[] MatrixChars =
    {
        'ｦ', 'ｧ', 'ｨ', 'ｩ', 'ｪ', 'ｫ', 'ｬ', 'ｭ', 'ｮ', 'ｯ',
        'ｰ', 'ｱ', 'ｲ', 'ｳ', 'ｴ', 'ｵ', 'ｶ', 'ｷ', 'ｸ', 'ｹ',
        'ｺ', 'ｻ', 'ｼ', 'ｽ', 'ｾ', 'ｿ', 'ﾀ', 'ﾁ', 'ﾂ', 'ﾃ',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'Z', 'X', 'C', 'V', 'B', 'N', 'M'
    };

    public MatrixColumn(int x, int maxHeight, Random random)
    {
        this.x = x;
        this.maxHeight = maxHeight;
        this.random = random;
        this.y = random.Next(-50, 0);
        this.length = random.Next(10, 30);
        this.speed = random.Next(1, 3);
        this.chars = new List<char>();

        for (int i = 0; i < length; i++)
        {
            chars.Add(MatrixChars[random.Next(MatrixChars.Length)]);
        }
    }

    public void Update()
    {
        y += speed;

        if (y - length > maxHeight)
        {
            y = random.Next(-50, -10);
            length = random.Next(10, 30);
            chars.Clear();
            for (int i = 0; i < length; i++)
            {
                chars.Add(MatrixChars[random.Next(MatrixChars.Length)]);
            }
        }

        // Ubah karakter secara acak
        if (random.Next(100) < 10)
        {
            int idx = random.Next(chars.Count);
            chars[idx] = MatrixChars[random.Next(MatrixChars.Length)];
        }
    }

    public void Render(Canvas canvas)
    {
        for (int i = 0; i < length; i++)
        {
            int currentY = y - i;
            if (currentY >= 0 && currentY < maxHeight)
            {
                Color color;
                if (i == 0)
                {
                    color = Color.White; // Karakter paling depan putih terang
                }
                else if (i < 5)
                {
                    color = Color.Lime; // Hijau terang
                }
                else if (i < 10)
                {
                    color = Color.Green; // Hijau medium
                }
                else
                {
                    color = Color.DarkGreen; // Hijau gelap
                }

                canvas.SetPixel(x, currentY, color);
            }
        }
    }
}