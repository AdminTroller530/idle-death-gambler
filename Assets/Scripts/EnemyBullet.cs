using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 0f;
    public float damage = 0f;
    float lifetime = 3f;
    BoxCollider2D col;
    public PlayerHealth playerHealth;

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
        if (other.gameObject.tag == "Player")
        {
            playerHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        
    }
}
