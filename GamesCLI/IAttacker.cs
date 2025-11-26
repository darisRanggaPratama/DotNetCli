namespace GamesCLI;

public interface IAttacker
{
    int AttackPower { get; }
    void Attack(IDamageable target);
}