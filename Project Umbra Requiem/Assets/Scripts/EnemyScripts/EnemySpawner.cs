using NUnit.Framework;
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
        public int enemyCount; //O número de inimigos a spawnar nessa wave
        public int spawnCount; //O número de inimigos desse tipo q já spawnaram nessa wave
        public GameObject enemyPrefab;
    }


    public List<Wave> waves; //Lista de todas as waves do jogo
    public int currentWaveCount;

    [Header("Spawner Attributes")]
    float spawnTimer; //Timer para determinar quando spawnar o proximo enemy
    public float waveInterval; //Intervalo entre as waves
    public int enemiesAlive;
    public int maxEnemiesAllowed;
    public bool maxEnemiesReached;

    [Header("SpawnPositions")]
    public List<Transform> relativeSpawnPoints;

    Transform player;

    void Start()
    {
        CalculateWaveQuota();
        player = FindAnyObjectByType<PlayerStats>().transform;
    }

    void Update()
    {
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0)
        {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer = Time.time;

        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            SpawnEnemies();
            spawnTimer = 0f;
        }
    }

    IEnumerator BeginNextWave()
    {
        yield return new WaitForSeconds(waveInterval);

        if (currentWaveCount < waves.Count -1)
        {
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;

    }
    
    void SpawnEnemies()
    {
        //Checa se o número minimo de inimigos na wave ja spawnou
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            //Spawna cada tipo de inimigo até a fila estar cheia
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                //Checa se o número minimo de inigos desse tipo já spawnou
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }

                    Instantiate(enemyGroup.enemyPrefab, player.transform.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);

                    

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
        enemiesAlive--;
    }
}
