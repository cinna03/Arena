using UnityEngine;

// OOP: Inheritance - base class for all enemy types
// OOP: Abstraction - implements IDamageable
// DESIGN PATTERN: State pattern (Idle, Chase, Attack)
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float moveSpeed = 2f;
    public int health = 2;
    public int damage = 1;
    public int scoreValue = 10;

    // STATE PATTERN: enemy states
    protected enum EnemyState { Idle, Chase, Attack }
    protected EnemyState currentState = EnemyState.Idle;

    protected Transform _player;
    private float _attackCooldown = 1f;
    private float _nextAttackTime;

    // Flash effect
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    protected virtual void Start()
    {
        // ALGORITHM: Search - find player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            _player = playerObj.transform;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer != null)
            _originalColor = _spriteRenderer.color;

        currentState = EnemyState.Chase;
    }

    protected virtual void Update()
    {
        // STATE PATTERN: behaviour changes based on current state
        switch (currentState)
        {
            case EnemyState.Chase:
                ChasePlayer();
                break;
            case EnemyState.Attack:
                AttackPlayer();
                break;
        }
    }

    private void ChasePlayer()
    {
        if (_player == null) return;
        transform.position = Vector2.MoveTowards(
            transform.position,
            _player.position,
            moveSpeed * Time.deltaTime
        );
    }

    private void AttackPlayer()
    {
        if (_player == null) return;

        // Keep moving toward player while attacking
        transform.position = Vector2.MoveTowards(
            transform.position,
            _player.position,
            moveSpeed * Time.deltaTime
        );

        // Deal damage on cooldown
        if (Time.time >= _nextAttackTime)
        {
            _nextAttackTime = Time.time + _attackCooldown;
            IDamageable target = _player.GetComponent<IDamageable>();
            if (target != null)
            {
                Debug.Log("Enemy attacking player!");
                target.TakeDamage(damage);
            }
        }
    }

    // Trigger enter - switch to attack state
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy reached player - switching to Attack");
            currentState = EnemyState.Attack;
        }
    }

    // Trigger stay - keep attacking while touching player
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            currentState = EnemyState.Attack;
    }

    // Trigger exit - switch back to chase
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy lost player - switching to Chase");
            currentState = EnemyState.Chase;
        }
    }

    // ABSTRACTION: IDamageable implementation
    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        if (_spriteRenderer != null)
            StartCoroutine(FlashWhite());
        if (!IsAlive())
            Die();
    }

    public bool IsAlive() => health > 0;

    protected virtual void Die()
    {
        GameManager.Instance.AddScore(scoreValue);
        GameManager.Instance.AddKill();
        Destroy(gameObject);
    }

    private System.Collections.IEnumerator FlashWhite()
    {
        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = _originalColor;
    }
}
