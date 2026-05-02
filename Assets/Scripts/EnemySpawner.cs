using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    List<GameObject> currentEnemies = new List<GameObject>();
    [SerializeField] List<EnemyWave> waves;
    [SerializeField] float timeBetweenSpawns, timeBetweenWaves;
    bool waveSpawnDone = false;
    int currentWave = 0;

    void SpawnEnemy(int id, Vector2 pos)
    {
        currentEnemies.Add(Instantiate(enemyPrefabs[id], pos, transform.rotation, transform));
    }

    void SpawnWave(EnemyWave wave)
    {
        List<EnemySpawn> spawns = wave.spawns;
        for (int i=0; i<spawns.Count; i++)
        {
            SpawnEnemy(spawns[i].id, spawns[i].pos);
        }
        waveSpawnDone = true;
    }

    void Start()
    {
        SpawnWave(waves[currentWave]);
    }

    void Update()
    {
        if (currentWave < waves.Count - 1 && waveSpawnDone && currentEnemies.TrueForAll(e => !e))
        {
            currentWave++;
            waveSpawnDone = false;
            SpawnWave(waves[currentWave]);
        }
        
    }
}
