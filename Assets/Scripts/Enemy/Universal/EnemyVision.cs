using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [System.NonSerialized] public bool CanSeePlayer = false;
    private RaycastHit2D _ray;
    private Transform _playerTransform;
    [SerializeField] LayerMask _wallMask;
    private const float CIRCLE_CAST_RADIUS = 0.6f;

    private void Start()
    {
        _playerTransform = PlayerManager.Instance.Transform;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, _playerTransform.position - transform.position, Color.red);
        _ray = Physics2D.CircleCast(transform.position, CIRCLE_CAST_RADIUS, _playerTransform.position - transform.position, Vector2.Distance(transform.position, _playerTransform.position) - CIRCLE_CAST_RADIUS, _wallMask);
        CanSeePlayer = !_ray.collider;
    }
}
