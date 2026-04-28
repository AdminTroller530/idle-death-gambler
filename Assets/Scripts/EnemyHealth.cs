using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    EnemyBase enemyBase;
    EnemyStats stats;
    float health;
    [SerializeField] TextMeshProUGUI healthText; //temp

    void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
        stats = enemyBase.stats;
        health = stats.maxHealth;
    }

    void Update()
    {
        healthText.text = ((int)health).ToString();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Death();
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
