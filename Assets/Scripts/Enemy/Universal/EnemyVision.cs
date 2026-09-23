using System.Collections;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    private EnemyBase _enemyBase;
    [System.NonSerialized] public bool CanSeePlayer = false;
    [System.NonSerialized] public bool CanShootPlayer = false;
    private RaycastHit2D _ray;
    private Transform _playerTransform;
    [SerializeField] LayerMask _wallMask;
    private const float CIRCLE_CAST_RADIUS_MOVE = 0.4f;
    private const float CIRCLE_CAST_RADIUS_SHOOT = 0.2f;

    private void Awake()
    {
        _enemyBase = GetComponent<EnemyBase>();
    }

    private void Start()
    {
        _playerTransform = PlayerManager.Instance.Transform;

        StartCoroutine(CheckVision());
    }

    private IEnumerator CheckVision()
    {
        // Debug.DrawRay(transform.position, _playerTransform.position - transform.position, Color.red);
        float rayRadius = CIRCLE_CAST_RADIUS_MOVE + _enemyBase.Stats.VisionRadiusExtra;
        _ray = Physics2D.CircleCast(transform.position, rayRadius, _playerTransform.position - transform.position, Vector2.Distance(transform.position, _playerTransform.position) - rayRadius, _wallMask);
        CanSeePlayer = ! (_ray.collider && Vector2.Distance(transform.position, _ray.point) < Vector2.Distance(transform.position, _playerTransform.position));

        rayRadius = CIRCLE_CAST_RADIUS_SHOOT;
        _ray = Physics2D.CircleCast(transform.position, rayRadius, _playerTransform.position - transform.position, Vector2.Distance(transform.position, _playerTransform.position) - rayRadius, _wallMask);
        CanShootPlayer = ! (_ray.collider && Vector2.Distance(transform.position, _ray.point) < Vector2.Distance(transform.position, _playerTransform.position));

        yield return new WaitForSeconds(0.1f); // cooldown
        StartCoroutine(CheckVision());
    }
}
