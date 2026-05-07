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

    [SerializeField] BoxCollider2D enterTrigger;
    [SerializeField] GameObject doors;
    bool started = false;

    void SpawnEnemy(int id, Vector2 pos)
    {
        GameObject enemy = Instantiate(enemyPrefabs[id], transform);
        enemy.transform.localPosition = pos;
        currentEnemies.Add(enemy);
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

    void StartWaves()
    {
        if (waves.Count == 0) return;
        waveTimer = timeBetweenWaves;
        // AudioController.UpdateLowPass(1);
        doors.SetActive(true);
        AstarPath.active.data.gridGraph.center = transform.position;
        AstarPath.active.Scan();
        StartCoroutine(SpawnWave(waves[currentWave]));
    }

    void Start()
    {
        // StartWaves();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!started && other.gameObject.tag == "Player")
        {
            if (enterTrigger.bounds.Contains(other.bounds.min) && enterTrigger.bounds.Contains(other.bounds.max))
            {
                started = true;
                StartWaves();
            }
        }
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
                // Debug.Log("waves defeated");
                // AudioController.UpdateLowPass(0);
                doors.SetActive(false);
                waveSpawnDone = false;
            }

        }
    }
}
