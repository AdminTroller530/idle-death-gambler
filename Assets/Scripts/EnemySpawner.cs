using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    List<GameObject> currentEnemies = new List<GameObject>();
    [SerializeField] List<EnemyWave> waves;
    [SerializeField] float timeBetweenSpawns, timeBetweenWaves;
    float waveTimer;
    bool waveSpawnDone = false;
    int currentWave = 0;

    void SpawnEnemy(int id, Vector2 pos)
    {
        currentEnemies.Add(Instantiate(enemyPrefabs[id], pos, transform.rotation, transform));
    }

    IEnumerator SpawnWave(EnemyWave wave)
    {
        waveSpawnDone = false;
        List<EnemySpawn> spawns = wave.spawns;
        for (int i=0; i<spawns.Count; i++)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            SpawnEnemy(spawns[i].id, spawns[i].pos);
        }
        waveSpawnDone = true;
    }

    void Start()
    {
        waveTimer = timeBetweenWaves;
        StartCoroutine(SpawnWave(waves[currentWave]));
    }

    void Update()
    {
        if (waveSpawnDone && currentEnemies.TrueForAll(e => !e))
        {

            if (currentWave < waves.Count - 1) // prepare to spawn next wave
            {
                if (waveTimer > 0) waveTimer -= Time.deltaTime;
                else {
                    currentWave++;
                    waveTimer = timeBetweenWaves;
                    StartCoroutine(SpawnWave(waves[currentWave]));
                }
            }
            else // all waves defeated
            {
                Debug.Log("waves defeated");
                Destroy(gameObject);
            }

        }
    }
}
