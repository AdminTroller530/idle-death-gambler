using UnityEngine;
using Pathfinding;
using UnityEngine.InputSystem;

public class EnemyMove : MonoBehaviour
{
    EnemyBase enemyBase;
    EnemyStats stats;
    Rigidbody2D rb;
    bool hasSeenPlayer = false;

    private Transform playerTransform;
    private EnemyVision _enemyVision;

    Vector2 knockback;
    float knockbackStunTimer = 0, knockbackStunTimerMax = 0.6f;

    AIPath path;

    void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
        _enemyVision = GetComponent<EnemyVision>();
        playerTransform = PlayerManager.Instance.Transform;
        rb = gameObject.GetComponent<Rigidbody2D>();
        path = GetComponent<AIPath>();
    }

    private void Start()
    {
        stats = enemyBase.stats;
    }

    void Update()
    {
        path.maxSpeed = stats.moveSpeed;
        if (!hasSeenPlayer && _enemyVision.CanSeePlayer) hasSeenPlayer = true;
        if (!hasSeenPlayer) path.maxSpeed *= 0.75f; // moves slower if hasn't seen player yet

        path.destination = playerTransform.position;
        if (_enemyVision.CanSeePlayer) path.endReachedDistance = 10;
        else path.endReachedDistance = 0;

        if (knockbackStunTimer > 0) knockbackStunTimer -= Time.deltaTime;
        else path.canMove = true; // allow pathfinding to continue once knockback stun done
    }

    void FixedUpdate()
    {
        // damp knockback velocity over time
        if (knockback.magnitude > 0.1f) knockback *= 0.85f;
        else knockback = Vector2.zero;
        
        rb.linearVelocity = knockback;
    }

    public void TakeKnockback(Vector2 dir, float magnitude)
    {
        knockback = dir.normalized * magnitude;
        knockbackStunTimer = knockbackStunTimerMax;
        path.canMove = false;
    }
}
