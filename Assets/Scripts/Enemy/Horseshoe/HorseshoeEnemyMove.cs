using UnityEngine;

public class HorseshoeEnemyMove : EnemyMove
{
    private bool _isLunging = false;
    private float _lungeSpeed = 30f;
    private Vector2 _lungeVector;

    protected override void Update()
    {
        base.Update();
        _path.canMove = !_isLunging;
    }

    protected override void ControlMovement()
    {
        base.ControlMovement();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (_isLunging)
        {
            _rigidbody.linearVelocity += _lungeVector;
        }
    }

    public void Lunge(Vector2 direction)
    {
        _isLunging = true;
        _lungeVector = direction.normalized * _lungeSpeed;
    }
}
