using UnityEngine;

// OOP: Inheritance - TankEnemy extends Enemy
// OOP: Polymorphism - overrides TakeDamage to show resistance
public class TankEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
        moveSpeed = 1f;
        health = 6;
        scoreValue = 30;
        damage = 2;
    }

    // POLYMORPHISM: TankEnemy takes reduced damage
    public override void TakeDamage(int amount)
    {
        int reducedDamage = Mathf.Max(1, amount - 1);
        base.TakeDamage(reducedDamage);
    }
}