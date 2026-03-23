using UnityEngine;
using System.Collections;
using TMPro;

// ALGORITHM: Wave spawning - escalating difficulty over time
// DESIGN PATTERN: Factory-style instantiation
public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject basicEnemyPrefab;
    public GameObject fastEnemyPrefab;
    public GameObject tankEnemyPrefab;

    [Header("Spawn Settings")]
    public float spawnRadius = 8f;
    private int _currentWave = 0;

    [Header("UI")]
    public TextMeshProUGUI waveText;

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    // ALGORITHM: Wave logic - each wave spawns more and harder enemies
    private IEnumerator SpawnWaves()
    {
        while (true)
        {
            _currentWave++;
            if (waveText != null)
                waveText.text = "Wave: " + _currentWave;
            yield return StartCoroutine(SpawnWave(_currentWave));
            yield return new WaitForSeconds(5f);
        }
    }

    private IEnumerator SpawnWave(int waveNumber)
    {
        int basicCount = waveNumber * 2;
        int fastCount  = waveNumber > 2 ? waveNumber - 1 : 0;
        int tankCount  = waveNumber > 4 ? 1 : 0;

        for (int i = 0; i < basicCount; i++)
        {
            SpawnEnemy(basicEnemyPrefab);
            yield return new WaitForSeconds(0.5f);
        }
        for (int i = 0; i < fastCount; i++)
        {
            SpawnEnemy(fastEnemyPrefab);
            yield return new WaitForSeconds(0.5f);
        }
        for (int i = 0; i < tankCount; i++)
        {
            SpawnEnemy(tankEnemyPrefab);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SpawnEnemy(GameObject prefab)
    {
        Vector2 spawnPos = Random.insideUnitCircle.normalized * spawnRadius;
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}