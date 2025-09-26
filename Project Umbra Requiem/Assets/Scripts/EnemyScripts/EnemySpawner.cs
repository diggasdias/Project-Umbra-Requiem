using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public int waveQuota;
        public float spawnInterval;
        public int spawnCount;
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount; // Número de inimigos a spawnar nessa wave
        public int spawnCount; // Número de inimigos desse tipo já spawnados nessa wave
        public GameObject enemyPrefab;
    }

    public List<Wave> waves; // Lista de todas as waves do jogo
    public int currentWaveCount;

    [Header("Spawner Attributes")]
    private float spawnTimer; // Timer para determinar quando spawnar o próximo inimigo
    public float waveInterval; // Intervalo entre as waves
    public int enemiesAlive;
    public int maxEnemiesAllowed;
    public bool maxEnemiesReached;

    [Header("SpawnPositions")]
    public List<Transform> relativeSpawnPoints;

    private Transform player;
    private bool isSpawningWave = false;

    void Start()
    {
        currentWaveCount = 0;
        CalculateWaveQuota();
        var playerStats = FindAnyObjectByType<PlayerStats>();
        if (playerStats != null)
            player = playerStats.transform;
        else
            Debug.LogError("PlayerStats não encontrado!");
    }

    void Update()
    {
        // Inicia a próxima wave apenas uma vez
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0 && !isSpawningWave)
        {
            StartCoroutine(BeginNextWave());
        }

        // Timer de spawn acumulativo
        spawnTimer += Time.deltaTime;

        if (currentWaveCount < waves.Count && spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            SpawnEnemies();
            spawnTimer = 0f;
        }
    }

    IEnumerator BeginNextWave()
    {
        isSpawningWave = true;
        yield return new WaitForSeconds(waveInterval);

        if (currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
            CalculateWaveQuota();
        }
        isSpawningWave = false;
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
            enemyGroup.spawnCount = 0; // Reset do spawnCount do grupo
        }
        waves[currentWaveCount].waveQuota = currentWaveQuota;
        waves[currentWaveCount].spawnCount = 0; // Reset do spawnCount da wave
    }

    void SpawnEnemies()
    {
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }

                    Vector3 spawnOffset = relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position;
                    Instantiate(enemyGroup.enemyPrefab, player.transform.position + spawnOffset, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;
                }
            }
        }

        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }

    public void OnEnemyKilled()
    {
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
    }
}
