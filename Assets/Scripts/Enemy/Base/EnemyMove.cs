using UnityEngine;
using Pathfinding;

public abstract class EnemyMove : MonoBehaviour
{
    protected EnemyBase _enemyBase;
    protected EnemyStats _stats;
    protected BoxCollider2D _boxCollider;
    protected Rigidbody2D _rigidbody;
    protected Animator _animator;
    protected bool _hasSeenPlayer = false;

    protected Transform _playerTransform;
    protected EnemyVision _enemyVision;

    protected Vector2 _currentKnockback;
    protected float _knockbackStunTimer = 0;
    protected float _knockbackStunTimerMax = 0.6f;

    protected AIPath _path;

    protected virtual void Awake()
    {
        _enemyBase = GetComponent<EnemyBase>();
        _enemyVision = GetComponent<EnemyVision>();
        _animator = GetComponent<Animator>();
        _playerTransform = PlayerManager.Instance.Transform;
        _boxCollider = gameObject.GetComponent<BoxCollider2D>();
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _path = GetComponent<AIPath>();
    }

    protected virtual void Start()
    {
        _stats = _enemyBase.Stats;
    }

    private void ManageKnockbackStun()
    {
        if (_knockbackStunTimer > 0) _knockbackStunTimer -= Time.deltaTime;
        else _path.canMove = true; // allow pathfinding to continue once knockback stun done
    }

    protected virtual void ControlMovement()
    {
        _path.maxSpeed = _stats.MoveSpeed;
        if (!_hasSeenPlayer)  {
            _path.maxSpeed *= 0.75f; // moves slower if hasn't seen player yet
            if (_enemyVision.CanSeePlayer) _hasSeenPlayer = true;
        }

        _path.destination = _playerTransform.position;
        if (_enemyVision.CanSeePlayer) _path.endReachedDistance = _stats.PreferredDistanceFromPlayer;
        else _path.endReachedDistance = 0;
        
        if (_animator.runtimeAnimatorController) _animator.SetBool("IsWalking", !_path.reachedEndOfPath);
    }

    protected virtual void Update()
    {
        if (_enemyBase.IsDead)
        {
            _boxCollider.enabled = false;
            _rigidbody.linearVelocity = Vector2.zero;
            _path.canMove = false;
            return;
        }

        ControlMovement();
        ManageKnockbackStun();
    }

    protected virtual void FixedUpdate()
    {
        if (_enemyBase.IsDead) return;

        // damp knockback velocity over time
        if (_currentKnockback.magnitude > 0.1f) _currentKnockback *= 0.85f;
        else _currentKnockback = Vector2.zero;
        
        _rigidbody.linearVelocity = _currentKnockback;
    }

    public virtual void TakeKnockback(Vector2 dir, float magnitude)
    {
        _currentKnockback = dir.normalized * magnitude;
        _knockbackStunTimer = _knockbackStunTimerMax;
        _path.canMove = false;
    }
}
