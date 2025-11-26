namespace GamesCLI;

public class Enemy: Character
{
    public int ExperienceReward { get; private set; }
    public int GoldReward { get; private set; }
    public List<Item> LootTable { get; private set; }
    
    public Enemy(string name, int maxHealth, int attackPower, int expReward, int goldReward) : base(name, maxHealth, attackPower)
    {
        ExperienceReward = expReward;
        GoldReward = goldReward;
        LootTable = new List<Item>();
    }

    protected override void OnDeath()
    {
        Show.msg($"\n💀 {Name} telah dikalahkan!");
    }
    
    public void AddLoot(Item item)
    {
        LootTable.Add(item);
    }

    public Item DropLoot()
    {
        if (LootTable.Count == 0) return null;
        
        Random random = new Random();
        if (random.Next(100) < 50) // 50% chance to drop loot
        {
            Item item = LootTable[random.Next(0, LootTable.Count)];
            Show.msg($"\n🎁 {Name} menjatuhkan item: {item.Name}!");
            return item;
        }
        return null;
    }
}