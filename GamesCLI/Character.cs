namespace GamesCLI;

public abstract class Character: IDamageable, IAttacker
{
    private int _health;
    private int _maxHealth;
    private int _attackPower;
    
    public string Name { get; protected set; }

    public int Health
    {
        get => _health;
        set => _health = Math.Max(0, Math.Min(value, _maxHealth));
    }

    public int MaxHealth
    {
        get => _maxHealth;
        protected set => _maxHealth = value;
    }

    public int AttackPower
    {
        get => _attackPower;
        protected set => _attackPower = value;
    }
    
    public bool IsAlive => Health > 0;
    
    public void TakeDamage(int damage)
    {
        if (damage < 0) throw new ArgumentException("Damage tidak boleh negatif.");
        Health -= damage;
        Show.msg($"{Name} menerima {damage} kerusakan. Sisa kesehatan: {Health}/{MaxHealth}");

        if (!IsAlive)
        {
            OnDeath();
        }
    }
    
    protected abstract void OnDeath();
    
    public virtual void Attack(IDamageable target)
    {
        if (!IsAlive)
        {
            Show.msg($"{Name} tidak dapat menyerang karena sudah mati.");
            return;
        }
        
        Show.msg($"{Name} menyerang dengan kekuatan {AttackPower}.");
        target.TakeDamage(AttackPower);
    }
    
    protected Character(string name, int maxHealth, int attackPower)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        MaxHealth = maxHealth;
        Health = maxHealth;
        AttackPower = attackPower;
    }
    
    public virtual void DisplayStatus()
    {
        Show.msg($"Nama: {Name}");
        Show.msg($"Kesehatan: {Health}/{MaxHealth}");
        Show.msg($"Kekuatan Serangan: {AttackPower}");
    }
}