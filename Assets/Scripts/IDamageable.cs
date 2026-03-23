// OOP: Abstraction - defines a contract without implementation details
// Both Player and Enemy implement this interface
public interface IDamageable
{
    void TakeDamage(int amount);
    bool IsAlive();
}