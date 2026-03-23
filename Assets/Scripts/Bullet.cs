using UnityEngine;

// OOP: Encapsulation - bullet manages its own movement and collision
public class Bullet : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 1;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.linearVelocity = transform.up * speed;
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore the player
        if (other.CompareTag("Player")) return;

        // Try to damage anything that is IDamageable
        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}