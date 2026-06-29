using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    [SerializeField] EnemyBullet enemyBullet;
    EnemyBase enemyBase;
    EnemyStats stats;
    float shootCooldown = 0.7f;

    private EnemyVision _enemyVision;

    private Transform _playerTransform;
    private PlayerHealth _playerHealth;

    private void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();

        _enemyVision = GetComponent<EnemyVision>();
    }

    private void Start()
    {
        _playerTransform = PlayerManager.Instance.Transform;
        _playerHealth = PlayerManager.Instance.Health;
        stats = enemyBase.stats;
    }

    private void ShootBullet(float angleOffset)
    {
        float angle = Mathf.Atan2(_playerTransform.position.y - transform.position.y, _playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg;
        angle += angleOffset + Random.Range(-stats.shootInaccuracy, stats.shootInaccuracy);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        EnemyBullet bullet = EnemyBulletPool.Instance.BulletPool.Get();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = rotation;
        bullet.Initialize(stats.bulletSpeed, stats.bulletDamage, stats.bulletLifetime, stats.bulletSprites, stats.bulletStartOffset, _playerHealth);
    }
    
    private void Update()
    {
        if (shootCooldown > 0) shootCooldown -= Time.deltaTime;
        else shootCooldown = 0;

        if (shootCooldown == 0 && _enemyVision.CanSeePlayer)
        {
            if (stats.type != "melee") // VERY TEMPORARY SYSTEM
            {
                ShootBullet(0);
                if (stats.type == "shoot_triple")
                {
                    ShootBullet(-20);
                    ShootBullet(20);
                }
            
            }
            
            shootCooldown = stats.shootCooldown + Random.Range(-stats.shootCooldownOffsetMax, stats.shootCooldownOffsetMax);
        }
    }
}
