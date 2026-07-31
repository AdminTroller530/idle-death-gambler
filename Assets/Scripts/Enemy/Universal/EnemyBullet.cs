using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class EnemyBullet : MonoBehaviour
{
    private PlayerHealth _playerHealth;

    private float _speed;
    private int _damage;
    private float _lifetime;
    private float _maxLifetime;
    private Sprite[] _sprites;

    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private Vector2 _mousePos;
    private bool _isParried = false;

    private ObjectPool<EnemyBullet> _bulletPool;
    private bool _isReturned = false;

    public void Initialize(EnemyStats stats)
    {
        _speed = stats.BulletSpeed;
        _damage = stats.BulletDamage;
        _lifetime = stats.BulletLifetime;
        _maxLifetime = stats.BulletLifetime;
        _sprites = stats.BulletSprites;
        _collider.offset = stats.BulletHitboxOffset;
        _collider.size = stats.BulletHitboxSize;
        _playerHealth = PlayerManager.Instance.Health;

        transform.Translate(Vector2.right * stats.BulletStartOffset);
    }

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _spriteRenderer.sprite = _sprites[0];
        _bulletPool = EnemyBulletPool.Instance.BulletPool;
    }

    private void OnEnable()
    {
        _isParried = false;
        _isReturned = false;
    }

    private void TryReturnToPool()
    {
        if (!_isReturned) {
            _isReturned = true;
            _bulletPool.Release(this);
        }
    }

    private void Update()
    {
        transform.Translate(Vector2.right * _speed * Time.deltaTime);

        _lifetime -= Time.deltaTime;
        if (_lifetime < 0) TryReturnToPool();

        if (_isParried) _spriteRenderer.sprite = _sprites[1];
        else _spriteRenderer.sprite = _sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isParried && other.gameObject.tag == "Player")
        {
            if (PlayerParry.IsParrying)
            {
                _mousePos = CursorTracker.Pos;
                float angle = Mathf.Atan2(_mousePos.y - transform.position.y, _mousePos.x - transform.position.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);

                _lifetime = _maxLifetime;
                _speed *= 1.5f;
                _damage *= 2;
                PlayerParry.WasParrySuccessful = true;
                _isParried = true;
            }
            else {
                _playerHealth.TakeEnemyDamage(gameObject, (int)_damage);
                TryReturnToPool();
            }
        }
        if (_isParried && other.gameObject.tag == "Enemy")
        {
            other.GetComponent<EnemyHealth>().TakeDamage(_damage);
            TryReturnToPool();
        }
        else if (other.gameObject.tag == "Wall")
        {
            TryReturnToPool();
        }
    }
}
