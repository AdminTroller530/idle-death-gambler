using UnityEngine;
using Pathfinding;
using UnityEngine.InputSystem;

public class EnemyMove : MonoBehaviour
{
    EnemyStats stats;
    Rigidbody2D rb;
    GameObject p;
    float pDist;
    float pDistThreshold = 10f;
    Vector2 pMove;
    AIPath path;

    void Awake()
    {
        p = GetComponent<EnemyBase>().p;
        rb = gameObject.GetComponent<Rigidbody2D>();
        stats = GetComponent<EnemyBase>().stats;
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

        path.destination = p.transform.position;
    }

    void FixedUpdate()
    {
        // rb.linearVelocity = pMove * stats.moveSpeed;
    }
}
