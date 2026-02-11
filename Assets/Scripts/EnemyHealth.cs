using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    EnemyStats stats;
    float health;

    void Awake()
    {
        stats = GetComponent<EnemyBase>().stats;
        health = stats.maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }
}
