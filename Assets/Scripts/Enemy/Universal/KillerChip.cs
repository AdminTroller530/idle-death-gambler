using UnityEngine;

public class KillerChip : MonoBehaviour
{
    private const float INITIAL_SPEED = 3f;
    private const float ACCELERATION = 0.07f;
    private float _speed;

    private Transform _playerTransform;

    private void OnEnable()
    {
        _speed = INITIAL_SPEED;
        WarpPortal.OnWarpToNextFloor += Despawn;
    }

    private void OnDisable() {WarpPortal.OnWarpToNextFloor -= Despawn;}

    private void Despawn()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        _playerTransform = PlayerManager.Instance.Transform;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position, _speed * Time.deltaTime);
        _speed += ACCELERATION * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerManager.Instance.Health.TakeEnemyDamage(gameObject, 999);
        }
    }
}
