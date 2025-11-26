using System.Dynamic;

namespace GamesCLI;

public sealed class GameManager
{
    private static GameManager _instance;
    private static readonly object _lock = new object();
    
    public Player CurrentPlayer { get; private set; }
    public bool IsGameRunning { get; private set; }
    
    private GameManager() { }

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GameManager();
                    }
                }
            }
            return _instance;
        }
    }

    public void StartGame()
    {
        Console.Clear();
        Show.msg("Welcome to the Game!");
        
        Show.msg("Enter your player name:");
        string playerName = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(playerName)) playerName = "Hero";
        
        CurrentPlayer = new Player(playerName);
        IsGameRunning = true;
        
        CurrentPlayer.AddItem(new HealthPotion(50));
        CurrentPlayer.AddItem(new HealthPotion(50));
        
        Show.msg($"\n🎮 Game started! Welcome, {playerName}!");
        Show.msg("Your adventure begins now...\n");
        
        Thread.Sleep(1500);
        MainMenu();
    }

    private void MainMenu()
    {
        while (IsGameRunning && CurrentPlayer.IsAlive)
        {
            Show.msg("\n=== Main Menu ===");
            Show.msg("1. Mulai pertempuran");
            Show.msg("2. Lihat status");
            Show.msg("3. Buka Inventory");
            Show.msg("4. Istirahat (Pulihkan HP penuh)");
            Show.msg("5. Keluar dari game");
            Show.msg("Pilih opsi (1-5):");
            
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    StartBattle();
                    break;
                case "2":
                    CurrentPlayer.DisplayStatus();
                    break;
                case "3":
                    InventoryMenu();
                    break;
                case "4":
                    Rest();
                    break;
                case "5":
                    IsGameRunning = false;
                    Show.msg("Game stopped!");
                    break;
                default:
                    Show.msg("Please enter a valid option.");
                    break;
            }

            if (choice != "5")
            {
                Show.msg("\nTekan tombol apa saja untuk melanjutkan...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
    
    private void StartBattle()
    {
        Enemy enemy = EnemyFactory.CreateEnemy(CurrentPlayer.Level);
        
        Console.Clear();
        Show.msg($"\n⚔️ Pertempuran dimulai! Musuh muncul: {enemy.Name} (HP: {enemy.Health}, ATK: {enemy.AttackPower})\n");
        enemy.DisplayStatus();
        
        Thread.Sleep(1500);

        while (CurrentPlayer.IsAlive && enemy.IsAlive)
        {
            Show.msg($"\n--Giliran Anda!--");
            Show.msg("1. Serang");
            Show.msg("2. Gunakan Item");
            Show.msg("3. Kabur");
            Show.msg("\nPilih aksi (1-3):");
            
            string action = Console.ReadLine();

            switch (action)
            {
                case "1":
                    CurrentPlayer.Attack(enemy);
                    break;
                case "2":
                    if (!UseItemInBattle())
                        continue;
                    break;
                case "3":
                    if (AttemptFlee())
                        return;
                    break;
                default:
                    Show.msg("Please enter a valid option.");
                    continue;
            }

            if (!enemy.IsAlive)
            {
                Victory(enemy);
                return;
            } 
            
            Thread.Sleep(1000);
            Show.msg("--Giliran Musuh!--");
            enemy.Attack(CurrentPlayer);
            Thread.Sleep(1000);
        }
        
        if (!CurrentPlayer.IsAlive)
        {
            Show.msg("\n💀 You have been defeated... Game Over!");
            IsGameRunning = false;
        }
    }
    
    private bool UseItemInBattle()
    {
        CurrentPlayer.ShowInventory();
        
        if (CurrentPlayer.Inventory.Count == 0)
            return false;
        
        Show.msg($"Masukkan nama item (atau 'cancal'): ");
        string? itemName = Console.ReadLine();

        if (itemName?.ToLower() == "cancal")
            return false;
        
        return CurrentPlayer.UseItem(itemName);
    }

    private bool AttemptFlee()
    {
        Random random = new Random();
        if (random.Next(100) < 50)
        {
            Show.msg("\n🏃‍♂️ Berhasil kabur dari pertempuran!");
            return true;
        }
        else
        {
            Show.msg("\n❌ Gagal kabur!");
            return false;
        }
    }
    
    private void Victory(Enemy enemy){
        Show.msg($"\n 🎉 You defeated {enemy.Name}!");
        CurrentPlayer.GainExperience(enemy.ExperienceReward);
        CurrentPlayer.AddGold(enemy.GoldReward);
        
        Item loot = enemy.DropLoot();
        if (loot != null)
        {
            CurrentPlayer.AddItem(loot);
        }
    }

    private void InventoryMenu()
    {
        CurrentPlayer.ShowInventory();

        if (CurrentPlayer.Inventory.Count > 0)
        {
            Show.msg($"\nGunakan item? (masukkan nama item atau 'cancal'): ");
            string? itemName = Console.ReadLine();

            if (itemName?.ToLower() != "cancal")
            {
                CurrentPlayer.UseItem(itemName);
            }
        }
    }
    
    private void Rest()
    {
        CurrentPlayer.Health = CurrentPlayer.MaxHealth;
        Show.msg($"\n😴 {CurrentPlayer.Name} beristirahat dan memulihkan kesehatan penuh!");
        Show.msg($"HP sekarang: {CurrentPlayer.Health}/{CurrentPlayer.MaxHealth}");
    }
}
