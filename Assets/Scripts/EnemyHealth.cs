using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    EnemyBase enemyBase;
    EnemyStats stats;
    float health;

    void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
        stats = enemyBase.stats;
        health = stats.maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }
}
