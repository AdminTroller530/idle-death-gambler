using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    float speed = 10f;
    float lifetime = 3f;
    float damage = 5f;
    BoxCollider2D col;
    PlayerHealth playerHealth;

    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
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
