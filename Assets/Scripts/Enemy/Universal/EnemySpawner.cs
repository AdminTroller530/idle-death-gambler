using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyPrefabs;
    private List<GameObject> _currentEnemies = new List<GameObject>();
    [SerializeField] private List<EnemyWave> _waves;
    [SerializeField] private float _timeBetweenSpawns;
    [SerializeField] private float _timeBetweenWaves;
    private float _waveTimer;
    private bool _waveSpawnDone = false;
    private int _currentWave = 0;

    [SerializeField] private BoxCollider2D _enterTrigger;
    [SerializeField] private GameObject _doors;
    private bool _wavesStarted = false;

    [SerializeField] private GameObject _enemySpawnIndicatorPrefab;
    private const float ENEMY_SPAWN_INDICATOR_TIME = 1f;

    public static event Action OnEnemyWaveCompleted;

    private IEnumerator SpawnEnemy(int id, Vector2 pos)
    {
        GameObject spawnIndicator = Instantiate(_enemySpawnIndicatorPrefab, transform);
        spawnIndicator.transform.localPosition = pos;

        yield return new WaitForSeconds(ENEMY_SPAWN_INDICATOR_TIME);
        Destroy(spawnIndicator);

        GameObject enemy = Instantiate(_enemyPrefabs[id], transform);
        enemy.transform.localPosition = pos;
        _currentEnemies.Add(enemy);
    }

    private IEnumerator SpawnWave(EnemyWave wave)
    {
        _waveSpawnDone = false;
        List<EnemySpawn> spawns = wave.Spawns;
        for (int i=0; i<spawns.Count - 1; i++)
        {
            yield return new WaitForSeconds(_timeBetweenSpawns);
            StartCoroutine(SpawnEnemy(spawns[i].Id, spawns[i].SpawnPointTransform.localPosition));
        }

        // spawn last enemy in wave and wait till said enemy appears
        yield return new WaitForSeconds(_timeBetweenSpawns);
        yield return StartCoroutine(SpawnEnemy(spawns[spawns.Count-1].Id, spawns[spawns.Count-1].SpawnPointTransform.localPosition));

        _waveSpawnDone = true;
    }

    private void StartWaves()
    {
        _wavesStarted = true;
        PlayerMovement.InCombat = true;
        _waveTimer = _timeBetweenWaves;
        AudioController.Instance.UpdateLowPass(1);
        _doors.SetActive(true);
        StartCoroutine(SpawnWave(_waves[_currentWave]));
    }

    public void OnEnterRoom()
    {
        if (!_wavesStarted && _waves.Count > 0)
        {
            StartWaves();
        }
    }

    private void Update()
    {
        if (_waveSpawnDone && _currentEnemies.TrueForAll(e => !e || e.GetComponent<EnemyBase>().IsDead))
        {
            _currentEnemies.Clear();
            if (_currentWave < _waves.Count - 1) // prepare to spawn next wave
            {
                if (_waveTimer > 0) _waveTimer -= Time.deltaTime;
                else {
                    _currentWave++;
                    _waveTimer = _timeBetweenWaves;
                    StartCoroutine(SpawnWave(_waves[_currentWave]));
                }
            }
            else // all waves defeated
            {
                // Debug.Log("waves defeated");
                AudioController.Instance.UpdateLowPass(0);
                _doors.SetActive(false);
                OnEnemyWaveCompleted?.Invoke();
                _waveSpawnDone = false;
                PlayerMovement.InCombat = false;
            }

        }
    }
}
