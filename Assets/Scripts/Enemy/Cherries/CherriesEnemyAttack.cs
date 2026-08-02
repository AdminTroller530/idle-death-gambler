using UnityEngine;

public class CherriesEnemyAttack : EnemyAttack
{
    [SerializeField] private EnemyStats _splitBulletStats;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void ShootBulletPattern()
    {
        EnemyBullet bullet = CreateBullet(transform.position, GetAngleToPlayer(), 0);
        bullet.OnTouchWall += OnBulletTouchWall;
    }

    protected override void OnBulletTouchWall(RaycastHit2D raycastHit, float angle)
    {
        float normalAngle = Mathf.Atan2(raycastHit.normal.y, raycastHit.normal.x) * Mathf.Rad2Deg;
        // CreateBullet(raycastHit.point, normalAngle, 0, _splitBulletStats);
        CreateBullet(raycastHit.point, normalAngle, -35, _splitBulletStats);
        CreateBullet(raycastHit.point, normalAngle, 35, _splitBulletStats);
    }
}
