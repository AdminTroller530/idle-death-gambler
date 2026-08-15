using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;

public class EnemyBullet : MonoBehaviour
{
    private PlayerHealth _playerHealth;
    private GameObject _originEnemy;

    private float _speed;
    private int _damage;
    private float _lifetime;
    private float _maxLifetime;
    private bool _hasWallDestroyParticles;
    private Sprite[] _sprites;

    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Light2D _light;
    private ParticleSystem _destroyParticles;

    private Vector2 _mousePos;
    private bool _isParried = false;

    private ObjectPool<EnemyBullet> _bulletPool;
    private bool _isDestroyed = false; // USE THIS TO IMPLEMENT DESTROY PARTICLES
    private bool _isReturned = false;

    private float _wallCollisionCooldown;

    [SerializeField] private LayerMask _wallMask;

    public event Action<RaycastHit2D, float> OnTouchWall; // <RaycastHit, angle>

    public void Initialize(EnemyStats stats, GameObject originEnemy)
    {
        _originEnemy = originEnemy;
        _speed = stats.BulletSpeed;
        _damage = stats.BulletDamage;
        _lifetime = stats.BulletLifetime;
        _maxLifetime = stats.BulletLifetime;
        _hasWallDestroyParticles = stats.BulletHasWallDestroyParticles;
        _sprites = stats.BulletSprites;
        _collider.offset = stats.BulletHitboxOffset;
        _collider.size = stats.BulletHitboxSize;
        _wallCollisionCooldown = stats.BulletWallCollisionCooldown;
        _playerHealth = PlayerManager.Instance.Health;

        transform.Translate(Vector2.right * stats.BulletStartOffset);
    }

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _light = GetComponent<Light2D>();
        _destroyParticles = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        _spriteRenderer.sprite = _sprites[0];
        _bulletPool = EnemyBulletPool.Instance.BulletPool;
    }

    private void OnEnable()
    {
        _isParried = false;
        _isDestroyed = false;
        _isReturned = false;
        _spriteRenderer.enabled = true;
        _light.enabled = true;

        var destroyParticlesColor = _destroyParticles.main;
        destroyParticlesColor.startColor = new ParticleSystem.MinMaxGradient(new Color(255, 0, 0), new Color(128, 0, 0));

        EnemySpawner.OnEnemyWaveCompleted += DestroyBulletFlip;
    }

    private void OnDisable()
    {
        EnemySpawner.OnEnemyWaveCompleted -= DestroyBulletFlip;
    }

    private void ReturnToPool()
    {
        if (_isReturned) return;

        _isReturned = true;
        OnTouchWall = null;
        _bulletPool.Release(this);
    }

    private void Update()
    {
        if (_isDestroyed) return;

        transform.Translate(Vector2.right * _speed * Time.deltaTime);

        _lifetime -= Time.deltaTime;
        if (_lifetime < 0) DestroyBulletFlip();

        if (_isParried) _spriteRenderer.sprite = _sprites[1];
        else _spriteRenderer.sprite = _sprites[0];
    }

    private void FlipRotationAngle()
    {
        transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + 180);
    }

    private void DestroyBulletFlip() {
        FlipRotationAngle();
        StartCoroutine(DestroyBullet());
    }

    private IEnumerator DestroyBullet()
    {
        if (_isDestroyed) yield break;
        _isDestroyed = true;
        
        _light.enabled = false;
        _spriteRenderer.enabled = false;
        _destroyParticles.Play();
        while (_destroyParticles.isPlaying) yield return null;
        
        ReturnToPool();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isDestroyed) return;

        if (!_isParried && other.gameObject.tag == "Player")
        {
            if (PlayerParry.IsParrying)
            {
                _mousePos = CursorTracker.Pos;
                float angle = Mathf.Atan2(_mousePos.y - transform.position.y, _mousePos.x - transform.position.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);

                _maxLifetime *= 3;
                _lifetime = _maxLifetime;
                _speed *= 1.5f;
                _damage *= 4;
                PlayerParry.WasParrySuccessful = true;
                _isParried = true;

                var destroyParticlesColor = _destroyParticles.main;
                destroyParticlesColor.startColor = new ParticleSystem.MinMaxGradient(new Color(255, 255, 255), new Color(128, 128, 128));
            }
            else {
                _playerHealth.TakeEnemyDamage(_originEnemy, (int)_damage);
                StartCoroutine(DestroyBullet());
            }
        }
        if (_isParried && other.gameObject.tag == "Enemy")
        {
            other.GetComponent<EnemyHealth>().TakeDamage(_damage);
            StartCoroutine(DestroyBullet());
        }
        else if (other.gameObject.tag == "Wall" && _maxLifetime - _lifetime >= _wallCollisionCooldown)
        {
            if (!_isParried)
            {
                RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, transform.rotation * Vector2.right, 1.2f, _wallMask);
                OnTouchWall?.Invoke(raycastHit, transform.eulerAngles.z);
            }

            if (_hasWallDestroyParticles || _isParried) StartCoroutine(DestroyBullet());
            else ReturnToPool();
        }
    }
}
