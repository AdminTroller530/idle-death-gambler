using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    [SerializeField] EnemyBullet enemyBullet;
    EnemyBase enemyBase;
    GameObject enemyBullets;
    EnemyStats stats;
    float shootCooldown = 0.7f;

    private EnemyVision _enemyVision;

    private Transform _playerTransform;
    private PlayerHealth _playerHealth;

    void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
        enemyBullets = GameObject.Find("Enemy Bullets");
        _enemyVision = GetComponent<EnemyVision>();
    }

    private void Start()
    {
        _playerTransform = PlayerManager.Instance.Transform;
        _playerHealth = PlayerManager.Instance.Health;
        stats = enemyBase.stats;
    }

    void Shoot(float angleOffset)
    {
        float angle = Mathf.Atan2(_playerTransform.position.y - transform.position.y, _playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg;
        angle += angleOffset + Random.Range(-stats.shootInaccuracy, stats.shootInaccuracy);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        
        EnemyBullet bullet = Instantiate(enemyBullet, transform.position, rotation, enemyBullets.transform);
        bullet.Initialize(stats.bulletSpeed, stats.bulletDamage, stats.bulletLifetime, stats.bulletSprites, stats.bulletStartOffset, _playerHealth);
    }
    
    void Update()
    {
        if (shootCooldown > 0) shootCooldown -= Time.deltaTime;
        else shootCooldown = 0;

        if (shootCooldown == 0 && _enemyVision.CanSeePlayer)
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
