namespace TextMatrix;

using System;
using System.Collections.Generic;

/// <summary>
/// Mengatur dan mengupdate animasi Matrix
/// </summary>
public class MatrixAnimator
{
    private readonly List<MatrixColumn> _columns;
    private readonly char[] _matrixChars;
    private readonly int _consoleWidth;
    private readonly int _consoleHeight;
    private readonly Random _random;
    private readonly AnimationConfig _config;
    private string _inputText;
    private int _animationFrame;

    public MatrixAnimator(int consoleWidth, int consoleHeight, string inputText, AnimationConfig? config = null)
    {
        _consoleWidth = consoleWidth;
        _consoleHeight = consoleHeight;
        _inputText = inputText;
        _random = new Random();
        _config = config ?? new AnimationConfig();
        _columns = new List<MatrixColumn>();
        _animationFrame = 0;

        // Karakter yang digunakan dalam animasi Matrix (katakana Jepang)
        _matrixChars = "ｦｧｨｩｪｫｬｭｮｯｰｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾗﾘﾘﾙﾚﾜﾜﾝ0123456789".ToCharArray();

        InitializeColumns();
    }

    /// <summary>
    /// Inisialisasi kolom-kolom jatuh berdasarkan density
    /// </summary>
    private void InitializeColumns()
    {
        int spacing = _config.Density == 1 ? 2 : Math.Max(1, 10 / _config.Density);
        
        for (int x = 0; x < _consoleWidth; x += spacing)
        {
            int startY = _random.Next(-_consoleHeight, 0);
            int speed = Math.Max(1, _config.Speed + _random.Next(-1, 2));
            int trailLength = _config.TrailLength + _random.Next(-2, 3);

            _columns.Add(new MatrixColumn(x, startY, _matrixChars, speed, Math.Max(5, trailLength)));
        }
    }

    /// <summary>
    /// Update state animasi untuk frame berikutnya
    /// </summary>
    public void Update()
    {
        _animationFrame++;

        foreach (var column in _columns)
        {
            column.Update();

            // Reset kolom jika sudah melampaui console
            if (column.Y > _consoleHeight)
            {
                column.Y = -column.TrailLength;
                column.CurrentIndex = 0;
            }
        }
    }

    /// <summary>
    /// Mendapatkan semua kolom untuk rendering
    /// </summary>
    public List<MatrixColumn> GetColumns()
    {
        return _columns;
    }

    /// <summary>
    /// Mendapatkan frame animasi saat ini
    /// </summary>
    public int GetCurrentFrame()
    {
        return _animationFrame;
    }

    /// <summary>
    /// Update input text untuk ditampilkan
    /// </summary>
    public void SetInputText(string text)
    {
        _inputText = text;
    }

    /// <summary>
    /// Mendapatkan input text
    /// </summary>
    public string GetInputText()
    {
        return _inputText;
    }

    /// <summary>
    /// Mendapatkan konfigurasi animasi
    /// </summary>
    public AnimationConfig GetConfig()
    {
        return _config;
    }
}

