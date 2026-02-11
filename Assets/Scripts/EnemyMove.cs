using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    EnemyStats stats;
    Rigidbody2D rb;
    GameObject p;
    float pDist;
    float pDistThreshold = 16f;
    Vector2 pMove;

    void Awake()
    {
        p = GameObject.Find("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
        stats = GetComponent<EnemyBase>().stats;
    }

    void Update()
    {
        pDist = Vector2.Distance(transform.position, p.transform.position);
        if (pDist > pDistThreshold)
        {
            pMove = (p.transform.position - transform.position).normalized;
        }
        else pMove = Vector2.zero;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = pMove * stats.moveSpeed;
    }
}
