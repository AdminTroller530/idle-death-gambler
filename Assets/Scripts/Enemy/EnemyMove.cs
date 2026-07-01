using UnityEngine;
using Pathfinding;

public class EnemyMove : MonoBehaviour
{
    private EnemyBase _enemyBase;
    private EnemyStats _stats;
    private Rigidbody2D _rb;
    private bool _hasSeenPlayer = false;

    private Transform _playerTransform;
    private EnemyVision _enemyVision;

    private Vector2 _knockback;
    private float _knockbackStunTimer = 0;
    private float _knockbackStunTimerMax = 0.6f;

    private AIPath _path;

    private void Awake()
    {
        _enemyBase = GetComponent<EnemyBase>();
        _enemyVision = GetComponent<EnemyVision>();
        _playerTransform = PlayerManager.Instance.Transform;
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _path = GetComponent<AIPath>();
    }

    private void Start()
    {
        _stats = _enemyBase.Stats;
    }

    private void Update()
    {
        _path.maxSpeed = _stats.moveSpeed;
        if (!_hasSeenPlayer && _enemyVision.CanSeePlayer) _hasSeenPlayer = true;
        if (!_hasSeenPlayer) _path.maxSpeed *= 0.75f; // moves slower if hasn't seen player yet

        _path.destination = _playerTransform.position;
        if (_enemyVision.CanSeePlayer) _path.endReachedDistance = 10;
        else _path.endReachedDistance = 0;

        if (_knockbackStunTimer > 0) _knockbackStunTimer -= Time.deltaTime;
        else _path.canMove = true; // allow pathfinding to continue once knockback stun done
    }

    private void FixedUpdate()
    {
        // damp knockback velocity over time
        if (_knockback.magnitude > 0.1f) _knockback *= 0.85f;
        else _knockback = Vector2.zero;
        
        _rb.linearVelocity = _knockback;
    }

    public void TakeKnockback(Vector2 dir, float magnitude)
    {
        _knockback = dir.normalized * magnitude;
        _knockbackStunTimer = _knockbackStunTimerMax;
        _path.canMove = false;
    }
}
