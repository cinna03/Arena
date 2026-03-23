using UnityEngine;
using TMPro;
using System.Collections;

// OOP: Encapsulation - health is private, modified only through TakeDamage()
// OOP: Abstraction - implements IDamageable interface
public class Player : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public float fireRate = 0.3f;
    private float _nextFireTime;

    [Header("Health")]
    [SerializeField] private int _maxHealth = 3;
    private int _currentHealth;

    [Header("UI")]
    public TextMeshProUGUI healthText;

    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentHealth = _maxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        UpdateHealthText();
    }

    private void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        _rb.linearVelocity = new Vector2(x, y).normalized * moveSpeed;

        // Rotate player to face mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        // CONFINE player to camera bounds
        Vector3 pos = transform.position;
        float camHeight = Camera.main.orthographicSize;
        float camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        pos.x = Mathf.Clamp(pos.x, -camWidth + 0.5f, camWidth - 0.5f);
        pos.y = Mathf.Clamp(pos.y, -camHeight + 0.5f, camHeight - 0.5f);
        transform.position = pos;
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButton(0) && Time.time >= _nextFireTime)
        {
            _nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        // Spawn bullet slightly in front of player to avoid self-hit
        Vector3 spawnPos = transform.position + transform.up * 0.5f;
        Instantiate(bulletPrefab, spawnPos, transform.rotation);
    }

    public void TakeDamage(int amount)
    {
        if (!IsAlive()) return;
        _currentHealth -= amount;
        _currentHealth = Mathf.Max(_currentHealth, 0);
        UpdateHealthText();
        StartCoroutine(FlashRed());
        if (!IsAlive())
            GameManager.Instance.TriggerGameOver();
    }

    public bool IsAlive() => _currentHealth > 0;

    private void UpdateHealthText()
    {
        if (healthText != null)
            healthText.text = "Lives: " + _currentHealth;
    }

    private IEnumerator FlashRed()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        _spriteRenderer.color = _originalColor;
    }
}
