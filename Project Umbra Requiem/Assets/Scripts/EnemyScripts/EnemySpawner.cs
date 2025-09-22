using NUnit.Framework;
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

    float spawnTimer;

    Transform player;

    void Start()
    {
        CalculateWaveQuota();
        player = FindAnyObjectByType<PlayerStats>().transform;
    }

    void Update()
    {
        spawnTimer = Time.time;

        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            SpawnEnemies();
            spawnTimer = 0f;
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
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota)
        {
            //Spawna cada tipo de inimigo até a fila estar cheia
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                //Checa se o número minimo de inigos desse tipo já spawnou
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    Vector2 spawnPosition = new Vector2(player.transform.position.x + Random.Range(-10f, 10f), player.transform.position.y + Random.Range(-10f, 10f));
                    Instantiate(enemyGroup.enemyPrefab, spawnPosition, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                }
            }
        }
    }
}
