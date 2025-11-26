namespace GamesCLI;

public class Player : Character, IInventoryHolder
{
    public int Level { get; private set; } 
    public int Experience { get; private set; }
    public int Gold { get; private set; }
    public List<Item> Inventory { get; private set; }
    
    private int ExperienceToNextLevel => Level * 100;
    
    public Player(string name) : base(name, 100, 15)
    {
        Level = 1;
        Experience = 0;
        Gold = 50;
        Inventory = new List<Item>();
    }
    
    public override void Attack(IDamageable target)
    {
        Show.msg($"\n{Name} melancarkan serangan!");
        base.Attack(target);
    }

    protected override void OnDeath()
    {
        Show.msg($"\n💀 {Name} telah gugur dalam pertempuran...");
        Show.msg("Game Over!");
    }
    
    public void GainExperience(int exp)
    {
        Experience += exp;
        Show.msg($"\n🪄 {Name} mendapatkan {exp} EXP.");

        while (Experience >= ExperienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Level++;
        Experience -= ExperienceToNextLevel;
        
        MaxHealth += 20;
        Health = MaxHealth;
        AttackPower += 5;
        
        Show.msg($"\n🎉 {Name} naik ke Level {Level}!");
        Show.msg($"❤️ Kesehatan maksimal meningkat menjadi {MaxHealth}.");
        Show.msg($"⚔️ Kekuatan serangan meningkat menjadi {AttackPower}.");
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        Show.msg($"\n💰 {Name} mendapatkan {amount} emas. Total emas: {Gold}");
    }
    
    public void AddItem(Item item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        
        Inventory.Add(item);
        Show.msg($"\n🎒 {item.Name} telah ditambahkan ke inventaris.");
    }

    public bool UseItem(string itemName)
    {
        Item? item = Inventory.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        
        if (item == null)
        {
            Show.msg($"\n❌ Item '{itemName}' tidak ditemukan di inventaris.");
            return false;
        }
        
        item.Use(this);
        Inventory.Remove(item);
        return true;
    }
    
    public override void DisplayStatus()
    {
        base.DisplayStatus();
        Show.msg($"Level: {Level}");
        Show.msg($"EXP: {Experience}/{ExperienceToNextLevel}");
        Show.msg($"Emas: {Gold}");
        Show.msg($"Jumlah Item di Inventaris: {Inventory.Count}");
    }
    
    public void ShowInventory()
    {
        Show.msg($"\n🎒 Inventaris {Name}:");
        if (Inventory.Count == 0)
        {
            Show.msg(" - Inventaris kosong.");
            return;
        }

        for (int i = 0; i < Inventory.Count; i++)
        {
            Show.msg($" {i + 1}. {Inventory[i].Name} - {Inventory[i].Description}");
        }
    }
}