using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    [SerializeField] private EnemyBullet _enemyBullet;
    private EnemyBase _enemyBase;
    private EnemyStats _stats;
    private float _shootCooldown = 0.7f;

    private EnemyVision _enemyVision;

    private Transform _playerTransform;
    private PlayerHealth _playerHealth;

    private void Awake()
    {
        _enemyBase = GetComponent<EnemyBase>();

        _enemyVision = GetComponent<EnemyVision>();
    }

    private void Start()
    {
        _playerTransform = PlayerManager.Instance.Transform;
        _playerHealth = PlayerManager.Instance.Health;
        _stats = _enemyBase.stats;
    }

    private void ShootBullet(float angleOffset)
    {
        float angle = Mathf.Atan2(_playerTransform.position.y - transform.position.y, _playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg;
        angle += angleOffset + Random.Range(-_stats.shootInaccuracy, _stats.shootInaccuracy);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        EnemyBullet bullet = EnemyBulletPool.Instance.BulletPool.Get();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = rotation;
        bullet.Initialize(_stats.bulletSpeed, _stats.bulletDamage, _stats.bulletLifetime, _stats.bulletSprites, _stats.bulletStartOffset, _playerHealth);
    }
    
    private void Update()
    {
        if (_shootCooldown > 0) _shootCooldown -= Time.deltaTime;
        else _shootCooldown = 0;

        if (_shootCooldown == 0 && _enemyVision.CanSeePlayer)
        {
            if (_stats.type != "melee") // VERY TEMPORARY SYSTEM
            {
                ShootBullet(0);
                if (_stats.type == "shoot_triple")
                {
                    ShootBullet(-20);
                    ShootBullet(20);
                }
            
            }
            
            _shootCooldown = _stats.shootCooldown + Random.Range(-_stats.shootCooldownOffsetMax, _stats.shootCooldownOffsetMax);
        }
    }
}
