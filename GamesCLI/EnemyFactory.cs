namespace GamesCLI;

public static class EnemyFactory
{
    private static readonly Random _random = new Random();
    public static Enemy CreateEnemy(int playerLevel)
    {
        (string, int, int, int, int)[] enemyTypes = new[]
        {
            ("Goblin", 30, 8, 25, 10),
            ("Orc", 50, 12, 40, 20),
            ("Troll", 80, 18, 60, 35),
            ("Skeleton", 120, 25, 100, 50),
            ("Zombie", 60, 8, 33, 15)
        };
        
        int maxIndex = Math.Min(playerLevel, enemyTypes.Length);
        var (name, hp, atk, exp, gold) = enemyTypes[_random.Next(maxIndex)];
        
        int scaledHp = hp + (playerLevel - 1) * 10;
        int scaledAtk = atk + (playerLevel - 1) * 2;
        int scaledExp = exp + (playerLevel - 1) * 10;
        int scaledGold = gold + (playerLevel - 1) * 5;

        Enemy enemy = new Enemy(name, scaledHp, scaledAtk, scaledExp, scaledGold);
        
        enemy.AddLoot(new HealthPotion(30));
        enemy.AddLoot(new HealthPotion(50));

        return enemy;
    }
    
}