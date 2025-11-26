namespace GamesCLI;

public interface IDamageable
{
    int Health { get; }
    int MaxHealth { get; }
    void TakeDamage(int damage);
    bool IsAlive { get; }
}