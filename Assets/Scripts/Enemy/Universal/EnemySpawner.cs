using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static RoomCardEnchantment;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyPrefabs;
    private List<EnemyBase> _currentEnemies = new List<EnemyBase>();
    [SerializeField] private List<EnemyWave> _waves;
    [SerializeField] private float _timeBetweenSpawns;
    [SerializeField] private float _timeBetweenWaves;
    private float _waveTimer;
    private bool _waveSpawnDone = false;
    private int _currentWave = 0;

    private RoomCardData _roomCard;

    private List<RoomCardEnchantment> _enchantments;

    [SerializeField] private BoxCollider2D _enterTrigger;
    [SerializeField] private GameObject _doors;
    private bool _wavesStarted = false;

    [SerializeField] private GameObject _enemySpawnIndicatorPrefab;
    private const float ENEMY_SPAWN_INDICATOR_TIME = 0.7f;

    public static event Action OnEnemyWaveCompleted;

    private RoomFadeIn _roomFadeIn;

    private void Awake()
    {
        _roomFadeIn = GetComponent<RoomFadeIn>();
    }

    public void SetRoomCard(RoomCardData roomCard) {_roomCard = roomCard;}

    public void SetEnchantments(List<RoomCardEnchantment> enchantments) {_enchantments = enchantments;}

    private IEnumerator SpawnEnemy(int id, Vector2 pos)
    {
        GameObject spawnIndicator = Instantiate(_enemySpawnIndicatorPrefab, transform);
        spawnIndicator.transform.localPosition = pos;

        yield return new WaitForSeconds(ENEMY_SPAWN_INDICATOR_TIME);
        Destroy(spawnIndicator);

        EnemyBase enemy = Instantiate(_enemyPrefabs[id], transform).GetComponent<EnemyBase>();
        enemy.transform.localPosition = pos;
        _currentEnemies.Add(enemy);
        ApplyEnchantmentsOnEnemy(enemy);
    }

    private void ApplyEnchantmentsOnEnemy(EnemyBase enemy)
    {
        for (int i = 0; i < _enchantments.Count; i++)
        {
            RoomCardEnchantment enchantment = _enchantments[i];
            
            if (enchantment == Greedy) enemy.Stats.ChipsDroppedMultiplier = 2;
        }
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
            StartCoroutine(_roomFadeIn.FadeIn());
        }
    }

    private void WavesDefeated()
    {
        AudioController.Instance.UpdateLowPass(0);
        _doors.SetActive(false);
        PlayerManager.Instance.Leveling.IncreaseXP(_roomCard.XPYield);
        OnEnemyWaveCompleted?.Invoke();

        _waveSpawnDone = false;
        PlayerMovement.InCombat = false;
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
            else
            {
                WavesDefeated();
            }

        }
    }
}
