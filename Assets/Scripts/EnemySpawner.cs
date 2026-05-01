using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    struct Wave
    {
        GameObject[] enemies;
        Vector2[] positions;
        float betweenCooldown;
    }
    
    [SerializeField] GameObject[] enemyPrefabs;
    List<GameObject> currentEnemies = new List<GameObject>();

    void SpawnEnemy(int id, Vector2 pos)
    {
        currentEnemies.Add(Instantiate(enemyPrefabs[id], pos, transform.rotation, transform));
    }

    void Start()
    {
        SpawnEnemy(0, new Vector2(17,0));
        SpawnEnemy(0, new Vector2(-13,5));
        SpawnEnemy(0, new Vector2(-18,-10));
        SpawnEnemy(1, new Vector2(6,-10));
    }

    void Update()
    {
        if (currentEnemies.TrueForAll(e => !e))
        {
            Debug.Log("all dead");
        }
        
    }
}
