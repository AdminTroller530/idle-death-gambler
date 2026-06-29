using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class EnemyBullet : MonoBehaviour
{
    private PlayerHealth _playerHealth;

    private float _speed;
    private float _damage;
    private float _lifetime;
    private float _maxLifetime;
    private Sprite[] _sprites;

    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private Vector2 _mousePos;
    private bool _isParried = false;

    private ObjectPool<EnemyBullet> _bulletPool;
    private bool _isReturned = false;

    public void Initialize(float speed, float damage, float lifetime, Sprite[] sprites, float startOffset, PlayerHealth playerHealth)
    {
        _speed = speed;
        _damage = damage;
        _lifetime = lifetime;
        _maxLifetime = lifetime;
        _sprites = sprites;
        _playerHealth = playerHealth;

        transform.Translate(Vector2.right * startOffset);
    }

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _sprites[0];
        _bulletPool = EnemyBulletPool.Instance.BulletPool;
        _playerHealth = PlayerManager.Instance.Health;
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
                _damage *= 1.5f;
                PlayerParry.WasParrySuccessful = true;
                _isParried = true;
            }
            else {
                _playerHealth.TakeDamage(_damage);
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
