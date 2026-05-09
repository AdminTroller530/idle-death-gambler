using UnityEngine;
using Pathfinding;
using UnityEngine.InputSystem;

public class EnemyMove : MonoBehaviour
{
    EnemyBase enemyBase;
    EnemyStats stats;
    Rigidbody2D rb;
    GameObject p;
    bool hasSeenPlayer = false;
    // float pDist;
    // float pDistThreshold = 10f;
    // Vector2 pMove;

    Vector2 knockback;
    float knockbackStunTimer = 0, knockbackStunTimerMax = 0.6f;

    AIPath path;

    void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
        p = enemyBase.p;
        rb = gameObject.GetComponent<Rigidbody2D>();
        stats = enemyBase.stats;
        path = GetComponent<AIPath>();
    }

    void Update()
    {
        // pDist = Vector2.Distance(transform.position, p.transform.position);
        // if (pDist > pDistThreshold)
        // {
        //     pMove = (p.transform.position - transform.position).normalized;
        // }
        // else pMove = Vector2.zero;

        path.maxSpeed = stats.moveSpeed;
        if (!hasSeenPlayer && enemyBase.seePlayer) hasSeenPlayer = true;
        if (!hasSeenPlayer) path.maxSpeed *= 0.75f; // moves slower if hasn't seen player yet

        path.destination = p.transform.position;
        if (enemyBase.seePlayer) path.endReachedDistance = 10;
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

    // NEED TO FIX: DIRECTION DOES NOT ACCOUNT FOR PLAYER SHOOTING INACCURACY
    public void TakeKnockback(Vector2 dir, float magnitude)
    {
        // Debug.Log("dir: " + dir + " magnitude: " + magnitude);
        knockback = dir.normalized * magnitude;
        knockbackStunTimer = knockbackStunTimerMax;
        path.canMove = false;
    }
}
