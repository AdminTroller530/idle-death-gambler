using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] public EnemyStats stats;
    public GameObject p;
    public bool seePlayer = false;
    RaycastHit2D ray;
    [SerializeField] LayerMask wallMask;

    void Awake()
    {
        p = GameObject.Find("Player");
    }

    void Update()
    {
        CheckSeePlayer();
    }

    void CheckSeePlayer()
    {
        Debug.DrawRay(transform.position, p.transform.position-transform.position, Color.red);
        ray = Physics2D.CircleCast(transform.position, 0.3f, p.transform.position-transform.position, Vector2.Distance(transform.position, p.transform.position), wallMask);
        seePlayer = !ray.collider;
    }
}
