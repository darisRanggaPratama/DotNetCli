namespace GameCLI;

public abstract class Character : ICharacter
{
    public string Name { get; protected set; }
    protected int Health { get; set; }
    public bool IsDead => Health <= 0;
    
    protected Character(string name, int health)
    {
        Name = name;
        Health = health;
    }
    
    public abstract void Attack(ICharacter target);

    public virtual void TakeDamage(int amount)
    {
        Health -= amount;
        Print.Text($"{Name} takes {amount} damage. Health: {Health}");
    }
}