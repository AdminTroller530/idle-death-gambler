using UnityEngine;

public class HorseshoeEnemyAttack : EnemyAttack
{
    private bool _isCharging = false;
    private float _chargeCooldown;
    private const float CHARGE_COOLDOWN_MAX = 1.5f;

    private HorseshoeEnemyMove _move;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        _move = GetComponent<HorseshoeEnemyMove>();
    }

    private void OnEnable()
    {
        _chargeCooldown = CHARGE_COOLDOWN_MAX;
    }

    protected override void Update()
    {
        if (_enemyBase.IsDead) return;

        if (!_isCharging)
        {
            if (_chargeCooldown > 0) _chargeCooldown -= CHARGE_COOLDOWN_MAX;
            if (_chargeCooldown <= 0 && _enemyVision.CanSeePlayer)
            {
                // run charge method in HorseshoeEnemyMove
                _isCharging = true;
            }
        }
        
    }

    protected override void ShootBulletPattern() // CHARGE AT PLAYER
    {
        throw new System.NotImplementedException();
    }
}
