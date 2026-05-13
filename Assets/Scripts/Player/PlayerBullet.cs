using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    float speed;
    float lifetime;
    float knockback;
    float damage;
    float startOffset;
    // BoxCollider2D col;

    public void Initialize(float speed, float lifetime, float knockback, float damage)
    {
        this.speed = speed;
        this.lifetime = lifetime;
        this.knockback = knockback;
        this.damage = damage;
    }

    void Start()
    {
        // col = GetComponent<BoxCollider2D>();
        transform.Translate(Vector2.right * startOffset);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        lifetime -= Time.deltaTime;
        if (lifetime <= 0) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            other.gameObject.GetComponent<EnemyMove>().TakeKnockback(transform.rotation * Vector2.right, knockback);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        
    }
}
