using UnityEngine;
using Pathfinding;
using UnityEngine.InputSystem;

public class EnemyMove : MonoBehaviour
{
    EnemyBase enemyBase;
    EnemyStats stats;
    Rigidbody2D rb;
    GameObject p;
    // float pDist;
    // float pDistThreshold = 10f;
    // Vector2 pMove;
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
        path.destination = p.transform.position;
        if (enemyBase.seePlayer) path.endReachedDistance = 10;
        else path.endReachedDistance = 0;
    }

    void FixedUpdate()
    {
        // rb.linearVelocity = pMove * stats.moveSpeed;
    }
}
