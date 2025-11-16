namespace GameCLI;

public class Player: Character
{
    private readonly Weapon _weapon;
    public Player(string name, Weapon weapon) : base(name, 100)
    {
        _weapon = weapon;
    }
    
    public override void Attack(ICharacter target)
    {
        int damage = _weapon.UseWeapon();
        Print.Text($"{Name} attacks with {_weapon.Name}");
        target.TakeDamage(damage);
    }
}