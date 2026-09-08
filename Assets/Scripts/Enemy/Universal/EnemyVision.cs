using System.Collections;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [System.NonSerialized] public bool CanSeePlayer = false;
    [System.NonSerialized] public bool CanShootPlayer = false;
    private RaycastHit2D _ray;
    private Transform _playerTransform;
    [SerializeField] LayerMask _wallMask;
    private const float CIRCLE_CAST_RADIUS_LARGE = 0.4f;
    private const float CIRCLE_CAST_RADIUS_SMALL = 0.2f;

    private void Start()
    {
        _playerTransform = PlayerManager.Instance.Transform;

        StartCoroutine(CheckVision());
    }

    private IEnumerator CheckVision()
    {
        // Debug.DrawRay(transform.position, _playerTransform.position - transform.position, Color.red);
        _ray = Physics2D.CircleCast(transform.position, CIRCLE_CAST_RADIUS_LARGE, _playerTransform.position - transform.position, Vector2.Distance(transform.position, _playerTransform.position) - CIRCLE_CAST_RADIUS_LARGE, _wallMask);
        CanSeePlayer = ! (_ray.collider && Vector2.Distance(transform.position, _ray.point) < Vector2.Distance(transform.position, _playerTransform.position));

        _ray = Physics2D.CircleCast(transform.position, CIRCLE_CAST_RADIUS_SMALL, _playerTransform.position - transform.position, Vector2.Distance(transform.position, _playerTransform.position) - CIRCLE_CAST_RADIUS_LARGE, _wallMask);
        CanShootPlayer = ! (_ray.collider && Vector2.Distance(transform.position, _ray.point) < Vector2.Distance(transform.position, _playerTransform.position));

        yield return new WaitForSeconds(0.1f); // cooldown
        StartCoroutine(CheckVision());
    }
}
