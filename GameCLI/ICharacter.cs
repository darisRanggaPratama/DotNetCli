namespace GameCLI;

public interface ICharacter
{
    void Attack(ICharacter target);
    void TakeDamage(int amount);
    bool IsDead { get;  }
    string Name { get; }
}