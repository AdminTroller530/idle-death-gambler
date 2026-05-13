using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    [SerializeField] EnemyBullet enemyBullet;
    EnemyBase enemyBase;
    GameObject enemyBullets;
    GameObject p;
    EnemyStats stats;
    float shootCooldown = 0.7f;

    void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
        p = EnemyBase.p;
        stats = enemyBase.stats;
        enemyBullets = GameObject.Find("Enemy Bullets");
    }

    void Shoot(float angleOffset)
    {
        float angle = Mathf.Atan2(p.transform.position.y - transform.position.y, p.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        angle += angleOffset + Random.Range(-stats.shootInaccuracy, stats.shootInaccuracy);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        
        EnemyBullet bullet = Instantiate(enemyBullet, transform.position, rotation, enemyBullets.transform);
        bullet.Initialize(stats.bulletSpeed, stats.bulletDamage, stats.bulletLifetime, stats.bulletSprites, stats.bulletStartOffset, p.GetComponent<PlayerHealth>());
    }
    
    void Update()
    {
        if (shootCooldown > 0) shootCooldown -= Time.deltaTime;
        else shootCooldown = 0;

        if (shootCooldown == 0 && enemyBase.seePlayer)
        {
            if (stats.type != "melee")
            {
                Shoot(0);
                if (stats.type == "shoot_triple")
                {
                    Shoot(-20);
                    Shoot(20);
                }
            
            }
            
            shootCooldown = stats.shootCooldown + Random.Range(-stats.shootCooldownOffsetMax, stats.shootCooldownOffsetMax);
        }
    }
}
