using UnityEngine;

public class HorseshoeEnemyMove : EnemyMove
{
    public bool IsLunging = false;
    private float _lungeSpeed = 20f;
    private Vector2 _lungeVector;
    private float _moveSpeedMultiplier = 1;

    protected override void Update()
    {
        base.Update();
        _path.canMove = !IsLunging;
        _path.maxSpeed *= _moveSpeedMultiplier;
    }

    protected override void ControlMovement()
    {
        base.ControlMovement();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (IsLunging)
        {
            _rigidbody.linearVelocity += _lungeVector;
        }
    }

    public void Lunge(Vector2 direction)
    {
        IsLunging = true;
        _moveSpeedMultiplier = 0.2f;
        _lungeVector = direction.normalized * _lungeSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            IsLunging = false;
            _lungeVector = Vector2.zero;
        }
    }

    public void ResetMoveSpeedMultiplier()
    {
        _moveSpeedMultiplier = 1;
    }
}
