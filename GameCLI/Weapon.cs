namespace GameCLI;

public class Weapon
{
    public string Name { get; }
    public int BaseDamage { get; }
    
    public Weapon(string name, int baseDamage)
    {
        Name = name;
        BaseDamage = baseDamage;
    }
    
    public int UseWeapon()
    {
        return BaseDamage + Randomzer.Get(1, 6);
    }
}