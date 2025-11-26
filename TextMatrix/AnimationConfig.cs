namespace TextMatrix;

/// <summary>
/// Konfigurasi untuk animasi Matrix
/// </summary>
public class AnimationConfig
{
    /// <summary>
    /// Durasi animasi dalam milidetik
    /// </summary>
    public int Duration { get; set; } = 5000;

    /// <summary>
    /// Delay antar frame dalam milidetik (untuk control FPS)
    /// </summary>
    public int FrameDelay { get; set; } = 50;

    /// <summary>
    /// Kecepatan kolom jatuh (1-5, semakin tinggi semakin cepat)
    /// </summary>
    public int Speed { get; set; } = 2;

    /// <summary>
    /// Panjang trail untuk setiap kolom
    /// </summary>
    public int TrailLength { get; set; } = 10;

    /// <summary>
    /// Densitas kolom (1-10, semakin tinggi semakin banyak)
    /// </summary>
    public int Density { get; set; } = 1;
}

