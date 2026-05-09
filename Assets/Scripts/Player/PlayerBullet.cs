using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    float speed = 16f;
    float lifetime = 3f;
    float knockback = 4f;
    float damage = 5f;
    BoxCollider2D col;

    public Vector2 dir;

    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
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
            other.gameObject.GetComponent<EnemyMove>().TakeKnockback(dir, knockback);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        
    }
}
