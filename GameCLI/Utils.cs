namespace GameCLI;

public static class Randomzer
{
    private static readonly Random _random = new();

    public static int Get(int min, int max) => _random.Next(min, max);
}