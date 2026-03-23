using UnityEngine;
using TMPro;

// DESIGN PATTERN: Observer - listens to GameManager events
// OOP: Encapsulation - UI logic is self-contained
public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI killsText;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalKillsText;

    private void Start()
    {
        gameOverPanel.SetActive(false);

        // OBSERVER: subscribe to GameManager events
        GameManager.Instance.OnScoreChanged += UpdateScore;
        GameManager.Instance.OnKillsChanged += UpdateKills;
        GameManager.Instance.OnGameOver     += ShowGameOver;

        // Initialize text
        scoreText.text = "Score: 0";
        killsText.text = "Kills: 0";
    }

    private void OnDestroy()
    {
        // Always unsubscribe to avoid memory leaks
        GameManager.Instance.OnScoreChanged -= UpdateScore;
        GameManager.Instance.OnKillsChanged -= UpdateKills;
        GameManager.Instance.OnGameOver     -= ShowGameOver;
    }

    private void UpdateScore(int newScore)
    {
        scoreText.text = "Score: " + newScore;
    }

    private void UpdateKills(int newKills)
    {
        killsText.text = "Kills: " + newKills;
    }

    private void ShowGameOver()
    {
        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + GameManager.Instance.Score;
        if (finalKillsText != null)
            finalKillsText.text = "Total Kills: " + GameManager.Instance.Kills;
        gameOverPanel.SetActive(true);
    }

    public void OnRestartButton()
    {
        GameManager.Instance.RestartGame();
    }
}