using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    [SerializeField] GameObject enemyBullet;
    GameObject enemyBullets;
    GameObject p;
    EnemyStats stats;

    void Awake()
    {
        p = GetComponent<EnemyBase>().p;
        stats = GetComponent<EnemyBase>().stats;
        enemyBullets = GameObject.Find("EnemyBullets");
    }

    void Shoot(float damage, float speed)
    {
        float angle = Mathf.Atan2(p.transform.position.y - transform.position.y, p.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        angle += Random.Range(-stats.shootInaccuracy, stats.shootInaccuracy);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        
        Instantiate(enemyBullet, transform.position, rotation, enemyBullets.transform);
    }
    
    // temporary debug enemy shooting test
    float temp = 0;
    void Update()
    {
        temp++;
        if (temp >= 60)
        {
            Shoot(stats.bulletDamage, stats.bulletSpeed);
            temp = 0;
        }
    }
}
