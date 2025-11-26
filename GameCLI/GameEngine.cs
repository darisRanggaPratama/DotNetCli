namespace GameCLI;

public class GameEngine
{
    private Player? _player;
    private Enemy? _enemy;

    public void Run()
    {
        Print.Text("Insert player name:");
        string name = Console.ReadLine() ?? "Player";
            _player = new Player(name, new Weapon("Sword", 10));
            Print.Text("\nGame started!");

            while (true)
            {
                SpawnEnergy();
                BattleLoop();
                
                Print.Text("Enemy Losed!");
                Print.Text("Continue? (y/n)");
                string? input = Console.ReadLine();
                if (input?.ToLower() != "y")
                {
                    Print.Text("Game ended!");
                    break;
                }

            }
    }

    private void BattleLoop()
    {
        while (!_player!.IsDead && !_enemy!.IsDead)
        {
            Print.Text("\n1. Attack 2. Defend");
            Print.Text("Choose action:");

            string? cmd = Console.ReadLine();

            if (cmd == "1")
            {
                _player.Attack(_enemy);
            }
            else
            {
                Print.Text($" {_player.Name} Defend!");
            }

            if (_enemy.IsDead)
            {
                Print.Text($" {_enemy.Name} Defeated!");
                break;
            }
            
            _enemy.Attack(_player);
            
            if (_player.IsDead)
            {
                Print.Text($" {_player.Name} Defeated!");
                Environment.Exit(0);
            }
        }
    }

    private void SpawnEnergy()
    {
        string[] names = ["Goblin", "Orc", "Troll", "Slime"];
        
        _enemy = new Enemy(name: names[Randomzer.Get(0, names.Length)], 
            health: Randomzer.Get(30, 70),
            baseDamage: Randomzer.Get(5, 12)
            );
        
        Print.Text($"Enemy {_enemy.Name} spawned! HP = ???");
    }
}