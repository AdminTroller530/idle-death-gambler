using UnityEngine;

public class SevenEnemyAttack : EnemyAttack
{
    private const int BULLET_SPREAD_ANGLE = 20;

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
        for (int angle = -BULLET_SPREAD_ANGLE*3; angle <= BULLET_SPREAD_ANGLE*3; angle += BULLET_SPREAD_ANGLE)
        {
            CreateBullet(transform.position, GetAngleToPlayer(), angle);
        }
    }
}
