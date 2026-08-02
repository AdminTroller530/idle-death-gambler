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

    protected float GetAngleToPlayer() => Mathf.Atan2(_playerTransform.position.y - transform.position.y, _playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg;

    protected virtual EnemyBullet CreateBullet(Vector2 position, float angle, float angleOffset, EnemyStats overrideStats = null)
    {
        EnemyStats stats = _stats;
        if (overrideStats) stats = overrideStats;

        angle += angleOffset + Random.Range(-stats.ShootInaccuracy, stats.ShootInaccuracy);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        EnemyBullet bullet = EnemyBulletPool.Instance.BulletPool.Get();
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.Initialize(stats);

        return bullet;
    }

    protected abstract void ShootBulletPattern();

    protected virtual void OnBulletTouchWall(RaycastHit2D raycastHit, float angle) {}
    
    protected virtual void Update()
    {
        if (_shootCooldown > 0) _shootCooldown -= Time.deltaTime;
        else _shootCooldown = 0;

        if (_shootCooldown == 0 && _enemyVision.CanSeePlayer)
        {
            ShootBulletPattern();
            _shootCooldown = _stats.ShootCooldown + Random.Range(-_stats.ShootCooldownOffsetMax, _stats.ShootCooldownOffsetMax);
        }
    }
}
