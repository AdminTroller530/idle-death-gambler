using UnityEngine;

public class ChipPrefab : MonoBehaviour
{
    private int _value;
    private bool _isCollected;

    private const float VELOCITY_OFFSET_MAX_X = 4f;
    private const float VELOCITY_OFFSET_MAX_Y = 5f;
    private const float GRAVITY = 0.4f;
    private const int FIXED_UPDATE_CALLS_MAX = 20;
    private int _fixedUpdateCalls;
    private Rigidbody2D _rigidbody;
    private Vector2 _velocity;
    private Transform _playerTransform;
    private const float PLAYER_MAGNET_DISTANCE = 3f;
    private float _playerMagnetSpeed;
    private const float PLAYER_MAGNET_ACCELERATION = 35f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _playerTransform = PlayerManager.Instance.Transform;
    }

    private void OnEnable()
    {
        _fixedUpdateCalls = 0;
        _isCollected = false;
        _playerMagnetSpeed = 0f;
        _velocity = new Vector2((Random.value > 0.5f ? 1 : -1) * Random.Range(VELOCITY_OFFSET_MAX_X/4, VELOCITY_OFFSET_MAX_X), Random.Range(VELOCITY_OFFSET_MAX_Y/2, VELOCITY_OFFSET_MAX_Y));
    }

    public void Initialize(int value)
    {
        _value = value;
    }

    private void FixedUpdate()
    {
        if (_fixedUpdateCalls < FIXED_UPDATE_CALLS_MAX)
        {
            _rigidbody.linearVelocity = _velocity;
            _velocity.y -= GRAVITY;
            _fixedUpdateCalls++;
        }
        else _rigidbody.linearVelocity = Vector2.zero;
    }

    private void Update()
    {
        if (_fixedUpdateCalls < FIXED_UPDATE_CALLS_MAX) return;

        if (Vector2.Distance(_playerTransform.position, transform.position) < PLAYER_MAGNET_DISTANCE)
        {
            transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position, _playerMagnetSpeed * Time.deltaTime);
            _playerMagnetSpeed += PLAYER_MAGNET_ACCELERATION * Time.deltaTime;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !_isCollected)
        {
            ChipsManager.Instance.IncreaseChips(_value);
            _isCollected = true;
            Destroy(gameObject);
        }
    }
}
