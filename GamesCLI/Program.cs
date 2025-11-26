namespace GamesCLI;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            GameManager game = GameManager.Instance;
            game.StartGame();

        }
        catch (Exception ex)
        {
            Show.msg($"☠️ An error occurred: {ex.Message}");
        }
        
        Show.msg($"Tekan sembarang tombol untuk keluar...");
        Console.ReadKey();
    }
}