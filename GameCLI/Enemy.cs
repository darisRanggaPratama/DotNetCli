namespace GameCLI;

public class Enemy: Character
{
    private readonly int _baseDamage;
    public Enemy(string name, int health, int baseDamage) : base(name, health)
    {
        _baseDamage = baseDamage;
    }
    
    public override void Attack(ICharacter target)
    {
        int damage = _baseDamage;
        Print.Text($"{Name} attacks player");
        target.TakeDamage(_baseDamage);
    }
}