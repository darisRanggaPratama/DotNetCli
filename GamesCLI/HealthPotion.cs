namespace GamesCLI;

public class HealthPotion: Item
{
    public int HealAmount { get; private set; }

    public HealthPotion(int healAmount = 50) : base("Health Potion", "Memulihkan Health Points")
    {
        HealAmount = healAmount;
    }

    public override void Use(Player player)
    {
        int healedAmount = Math.Min(HealAmount, player.MaxHealth - player.Health);
        player.Health += healedAmount;
        Show.msg($"\n🧪 {player.Name} menggunakan {Name} dan memulihkan {healedAmount} Health Points!");
        Show.msg($"HP saat ini: {player.Health}/{player.MaxHealth}");
    }
}