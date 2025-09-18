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
        public int enemyCount;
        public int spawnCount;
        public GameObject enemyPrefab;
    }


    public List<Wave> waves;
    public int currentWaveCount;

    void Start()
    {
        
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

                }
            }
        }
    }

    void Update()
    {
        
    }
}
