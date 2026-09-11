using UnityEngine;

public class HorseshoeEnemyAttack : EnemyAttack
{
    private float _chargeCooldown;
    private const float CHARGE_COOLDOWN_MAX = 1.5f;

    private HorseshoeEnemyMove _move;

    protected override void Awake()
    {
        base.Awake();
        _move = GetComponent<HorseshoeEnemyMove>();
    }

    private void OnEnable()
    {
        _chargeCooldown = CHARGE_COOLDOWN_MAX;
    }

    protected override void Update()
    {
        if (_enemyBase.IsDead) return;

        if (!_move.IsLunging)
        {
            if (_chargeCooldown > 0) _chargeCooldown -= Time.deltaTime;
            if (_chargeCooldown <= 0 && _enemyVision.CanSeePlayer)
            {
                _move.Lunge(_playerTransform.position - transform.position);
                _chargeCooldown = CHARGE_COOLDOWN_MAX;
            }
        }

        if (_chargeCooldown <= 0) _move.ResetMoveSpeedMultiplier();
    }

    protected override void ShootBulletPattern() // CHARGE AT PLAYER
    {
        throw new System.NotImplementedException();
    }
}
