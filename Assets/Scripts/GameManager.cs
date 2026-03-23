using UnityEngine;
using UnityEngine.SceneManagement;

// DESIGN PATTERN: Singleton - ensures only one GameManager exists
// OOP: Encapsulation - score and state managed through controlled methods
public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    // ENCAPSULATION: private backing fields, public read-only access
    private int _score = 0;
    private int _kills = 0;
    public int Score => _score;
    public int Kills => _kills;

    private bool _isGameOver = false;

    // OBSERVER PATTERN: UI listens to these events
    public event System.Action<int> OnScoreChanged;
    public event System.Action<int> OnKillsChanged;
    public event System.Action OnGameOver;

    private void Awake()
    {
        // Singleton enforcement
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddScore(int points)
    {
        if (_isGameOver) return;
        _score += points;
        OnScoreChanged?.Invoke(_score);
    }

    public void AddKill()
    {
        if (_isGameOver) return;
        _kills++;
        OnKillsChanged?.Invoke(_kills); // Notify observers
    }

    public void TriggerGameOver()
    {
        if (_isGameOver) return;
        _isGameOver = true;
        OnGameOver?.Invoke();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}