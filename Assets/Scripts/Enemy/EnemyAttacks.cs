using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    [SerializeField] GameObject enemyBullet;
    EnemyBase enemyBase;
    GameObject enemyBullets;
    GameObject p;
    EnemyStats stats;
    float shootCooldown = 0.7f;

    void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
        p = enemyBase.p;
        stats = enemyBase.stats;
        enemyBullets = GameObject.Find("Enemy Bullets");
    }

    void Shoot(float angleOffset)
    {
        float angle = Mathf.Atan2(p.transform.position.y - transform.position.y, p.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        angle += angleOffset + Random.Range(-stats.shootInaccuracy, stats.shootInaccuracy);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        
        GameObject bulletObject = Instantiate(enemyBullet, transform.position, rotation, enemyBullets.transform);
        EnemyBullet bullet = bulletObject.GetComponent<EnemyBullet>();
        bullet.damage = stats.bulletDamage;
        bullet.speed = stats.bulletSpeed;
        bullet.maxLifetime = stats.bulletLifetime;
        bullet.playerHealth = p.GetComponent<PlayerHealth>();
        bullet.sprites = stats.bulletSprites;
        bullet.startOffset = stats.bulletStartOffset;
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
