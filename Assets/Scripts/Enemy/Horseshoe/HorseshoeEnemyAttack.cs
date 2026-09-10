using UnityEngine;

public class HorseshoeEnemyAttack : EnemyAttack
{
    private bool _isLunging = false;
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

        if (!_isLunging)
        {
            if (_chargeCooldown > 0) _chargeCooldown -= Time.deltaTime;
            if (_chargeCooldown <= 0 && _enemyVision.CanSeePlayer)
            {
                // LUNGE DIRECTION MESSED UP SOMEHOW
                _move.Lunge(Vector2.MoveTowards(transform.position, _playerTransform.position, 1));
                _isLunging = true;
            }
        }
        
    }

    protected override void ShootBulletPattern() // CHARGE AT PLAYER
    {
        throw new System.NotImplementedException();
    }
}
