using UnityEngine;

public class CherriesEnemyAttack : EnemyAttack
{
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
        CreateBullet(0);
    }
}
