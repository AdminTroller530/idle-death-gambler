using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] EnemyStats stats; // IMPLEMENT AN ENEMY MANAGER SCRIPT TO STORE STATS PUBLICLY
    float health;

    void Awake()
    {
        health = stats.health;
        // health = GetComponent<EnemyMove>().stats.health; 
    }
}
