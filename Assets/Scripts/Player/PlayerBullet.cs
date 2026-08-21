using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;

public class PlayerBullet : MonoBehaviour
{
    private float _speed;
    private float _lifetime;
    private float _maxLifetime;
    private float _knockback;
    private float _damage;

    private bool _isDestroyed = false;
    private bool _isReturned = false;

    private SpriteRenderer _spriteRenderer;
    private Light2D _light;
    private ParticleSystem _destroyParticles;

    private ObjectPool<PlayerBullet> _bulletPool;

    public void Initialize(PlayerBaseStats stats)
    {
        _speed = stats.BulletSpeed;
        _lifetime = stats.BulletLifetime;
        _maxLifetime = stats.BulletLifetime;
        _knockback = stats.BulletKnockback;
        _damage = stats.ChipsPerShot;
        // transform.Translate(Vector2.right * startOffset);
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _light = GetComponent<Light2D>();
        _destroyParticles = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        _bulletPool = PlayerBulletPool.Instance.BulletPool;
    }

    private void OnEnable()
    {
        _isDestroyed = false;
        _isReturned = false;
        _spriteRenderer.enabled = true;
        _light.enabled = true;
    }

    private void ReturnToPool()
    {
        if (_isReturned) return;

        _isReturned = true;
        _bulletPool.Release(this);
    }

    private IEnumerator DestroyBullet()
    {
        if (_isDestroyed) yield break;
        _isDestroyed = true;
        
        _light.enabled = false;
        _spriteRenderer.enabled = false;
        _destroyParticles.Play();
        yield return new WaitUntil(() => _destroyParticles.isStopped);
        
        ReturnToPool();
    }

    private void Update()
    {
        if (_isDestroyed) return;

        transform.Translate(Vector2.right * _speed * Time.deltaTime);
        _lifetime -= Time.deltaTime;
        if (_lifetime <= 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + 180);
            StartCoroutine(DestroyBullet());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isDestroyed) return;

        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(_damage);
            other.gameObject.GetComponent<EnemyMove>().TakeKnockback(transform.rotation * Vector2.right, _knockback);
            StartCoroutine(DestroyBullet());
        }
        if (other.gameObject.tag == "Wall")
        {
            StartCoroutine(DestroyBullet());
        }
        
    }
}
