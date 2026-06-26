using UnityEngine;
using UnityEngine.Pool;

public class PlayerBullet : MonoBehaviour
{
    private float _speed;
    private float _lifetime;
    private float _knockback;
    private float _damage;
    private float _startOffset = 1.3f;

    private ObjectPool<PlayerBullet> _bulletPool;

    public void Initialize(float speed, float lifetime, float knockback, float damage)
    {
        _speed = speed;
        _lifetime = lifetime;
        _knockback = knockback;
        _damage = damage;
    }

    public void SetPool(ObjectPool<PlayerBullet> bulletPool)
    {
        _bulletPool = bulletPool;
    }

    private void Start()
    {
        transform.Translate(Vector2.right * _startOffset);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * _speed * Time.deltaTime);

        _lifetime -= Time.deltaTime;
        if (_lifetime <= 0) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(_damage);
            other.gameObject.GetComponent<EnemyMove>().TakeKnockback(transform.rotation * Vector2.right, _knockback);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        
    }
}
