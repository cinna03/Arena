// OOP: Inheritance - FastEnemy extends Enemy base class
// OOP: Polymorphism - overrides Start() to set different stats
public class FastEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
        // POLYMORPHISM: same enemy type, different behaviour
        moveSpeed = 4f;
        health = 1;
        scoreValue = 15;
    }
}