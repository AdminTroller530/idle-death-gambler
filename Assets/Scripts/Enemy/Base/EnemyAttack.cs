using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    protected EnemyStats _stats;
    protected float _shootCooldown = 0.7f; // initial value = how long it takes before firing first shot

    protected EnemyVision _enemyVision;

    protected Transform _playerTransform;
    protected PlayerHealth _playerHealth;

    protected virtual void Awake()
    {
        _enemyVision = GetComponent<EnemyVision>();
    }

    protected virtual void Start()
    {
        _playerTransform = PlayerManager.Instance.Transform;
        _playerHealth = PlayerManager.Instance.Health;
        _stats = GetComponent<EnemyBase>().Stats;
    }

    protected virtual void CreateBullet(float angleOffset)
    {
        float angle = Mathf.Atan2(_playerTransform.position.y - transform.position.y, _playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg;
        angle += angleOffset + Random.Range(-_stats.ShootInaccuracy, _stats.ShootInaccuracy);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        EnemyBullet bullet = EnemyBulletPool.Instance.BulletPool.Get();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = rotation;
        bullet.Initialize(_stats.BulletSpeed, _stats.BulletDamage, _stats.BulletLifetime, _stats.BulletSprites, _stats.BulletStartOffset, _playerHealth);
    }

    protected abstract void ShootBulletPattern();
    
    protected virtual void Update()
    {
        if (_shootCooldown > 0) _shootCooldown -= Time.deltaTime;
        else _shootCooldown = 0;

        if (_shootCooldown == 0 && _enemyVision.CanSeePlayer)
        {
            ShootBulletPattern();
            // CreateBullet(0);
            // if (_stats.Type == "shoot_triple")
            // {
            //     CreateBullet(-20);
            //     CreateBullet(20);
            // }

            _shootCooldown = _stats.ShootCooldown + Random.Range(-_stats.ShootCooldownOffsetMax, _stats.ShootCooldownOffsetMax);
        }
    }
}
