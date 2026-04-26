using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    [SerializeField] GameObject enemyBullet;
    EnemyBase enemyBase;
    GameObject enemyBullets;
    GameObject p;
    EnemyStats stats;
    float shootCooldown = 0.5f;

    void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
        p = enemyBase.p;
        stats = enemyBase.stats;
        enemyBullets = GameObject.Find("EnemyBullets");
    }

    void Shoot(float damage, float speed)
    {
        float angle = Mathf.Atan2(p.transform.position.y - transform.position.y, p.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        angle += Random.Range(-stats.shootInaccuracy, stats.shootInaccuracy);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        
        GameObject bulletObject = Instantiate(enemyBullet, transform.position, rotation, enemyBullets.transform);
        bulletObject.GetComponent<EnemyBullet>().playerHealth = p.GetComponent<PlayerHealth>();

    }
    
    // temporary debug enemy shooting test
    float temp = 0;
    void Update()
    {
        if (shootCooldown > 0) shootCooldown -= Time.deltaTime;
        else shootCooldown = 0;

        if (shootCooldown == 0 && enemyBase.seePlayer)
        {
            Shoot(stats.bulletDamage, stats.bulletSpeed);
            shootCooldown = stats.shootCooldown;
        }
    }
}
