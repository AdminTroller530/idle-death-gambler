using UnityEngine;
using UnityEngine.Pool;

public class ChipPrefab : MonoBehaviour
{
    private int _value;
    private bool _isCollected;

    private const float VELOCITY_OFFSET_MAX_X = 4f;
    private const float VELOCITY_OFFSET_MAX_Y = 5f;
    private const float GRAVITY = 0.4f;
    private const int FIXED_UPDATE_CALLS_MAX = 20;
    private int _fixedUpdateCalls;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private Vector2 _velocity;
    private Transform _playerTransform;

    private const float PLAYER_MAGNET_DISTANCE = 3.5f;
    private float _playerMagnetSpeed;
    private const float PLAYER_MAGNET_ACCELERATION = 35f;

    private ObjectPool<ChipPrefab> _chipPool;
    private bool _isReturned = false;

    [System.Serializable]
    public struct ChipSpriteMapping
    {
        public ChipValue ChipValue;
        public Sprite Sprite;
    }

    [SerializeField] private ChipSpriteMapping[] _chipSpriteMappings;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _playerTransform = PlayerManager.Instance.Transform;
        _chipPool = ChipsPool.Instance.ChipPool;
    }

    private void OnEnable()
    {
        _isReturned = false;
        _fixedUpdateCalls = 0;
        _isCollected = false;
        _playerMagnetSpeed = 0f;
        _velocity = new Vector2((Random.value > 0.5f ? 1 : -1) * Random.Range(VELOCITY_OFFSET_MAX_X/4, VELOCITY_OFFSET_MAX_X), Random.Range(VELOCITY_OFFSET_MAX_Y/2, VELOCITY_OFFSET_MAX_Y));
    }

    private void ReturnToPool()
    {
        _isReturned = true;
        _chipPool.Release(this);
    }

    public void Initialize(ChipValue value)
    {
        _value = (int)value;
        _spriteRenderer.sortingOrder = (int)value;

        foreach (ChipSpriteMapping chipSpriteMapping in _chipSpriteMappings)
        {
            if (chipSpriteMapping.ChipValue == value)
            {
                _spriteRenderer.sprite = chipSpriteMapping.Sprite;
                return;
            }
            _spriteRenderer.sprite = _chipSpriteMappings[0].Sprite; // fallback
        }
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
        if (_isReturned) return;

        if (other.gameObject.tag == "Player" && !_isCollected)
        {
            ChipsManager.Instance.IncreaseChips(_value);
            _isCollected = true;
            ReturnToPool();
        }
    }
}
