using UnityEngine;

public class KillerChip : MonoBehaviour
{
    private const float INITIAL_SPEED = 2f;
    private const float ACCELERATION = 0.05f;
    private float _speed;

    private Transform _playerTransform;

    private void Awake()
    {
        _speed = INITIAL_SPEED;
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
}
