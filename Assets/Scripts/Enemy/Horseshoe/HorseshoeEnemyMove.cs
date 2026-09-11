using UnityEngine;

public class HorseshoeEnemyMove : EnemyMove
{
    public bool IsLunging = false;
    private float _lungeSpeed;
    private const float LUNGE_INITIAL_SPEED = 0f;
    private const float LUNGE_ACCELERATION = 0.7f;
    private const float LUNGE_TERMINAL_SPEED = 28f;
    private const float LUNGE_RECHARGE_MOVE_SPEED_MULTIPLIER = 0.2f;
    private Vector2 _lungeVector;
    private float _moveSpeedMultiplier;

    private void OnEnable()
    {
        ResetMoveSpeedMultiplier();
    }

    protected override void Update()
    {
        base.Update();
        _path.maxSpeed *= _moveSpeedMultiplier;
    }

    protected override void ManageKnockbackStun()
    {
        base.ManageKnockbackStun();
        if (IsLunging) _path.canMove = false;
    }

    public override void TakeKnockback(Vector2 dir, float magnitude)
    {
        if (IsLunging) return;
        base.TakeKnockback(dir, magnitude);
    }

    protected override void ControlMovement()
    {
        base.ControlMovement();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (IsLunging && !_enemyBase.IsDead)
        {
            _rigidbody.linearVelocity += _lungeVector * _lungeSpeed;
            _lungeSpeed = Mathf.MoveTowards(_lungeSpeed, LUNGE_TERMINAL_SPEED, LUNGE_ACCELERATION);
        }
    }

    public void Lunge(Vector2 direction)
    {
        IsLunging = true;
        _lungeVector = direction.normalized;
        _lungeSpeed = LUNGE_INITIAL_SPEED;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            IsLunging = false;
            _lungeVector = Vector2.zero;
            _moveSpeedMultiplier = LUNGE_RECHARGE_MOVE_SPEED_MULTIPLIER;
        }
    }

    public void ResetMoveSpeedMultiplier() {_moveSpeedMultiplier = 1;}
}
