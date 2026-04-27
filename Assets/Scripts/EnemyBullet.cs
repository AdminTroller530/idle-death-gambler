using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyBullet : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public float speed = 0f;
    public float damage = 0f;
    public float maxLifetime = 0f, lifetime = 0f;
    public EnemyHealth enemyHealth;
    public Sprite[] sprites;

    BoxCollider2D col;
    SpriteRenderer s;

    Vector2 mousePos;
    bool parried = false;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        s = GetComponent<SpriteRenderer>();
        s.sprite = sprites[0];
        lifetime = maxLifetime;
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        lifetime -= Time.deltaTime;
        if (lifetime <= 0) Destroy(gameObject);

        if (parried) s.sprite = sprites[1];
        else s.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!parried && other.gameObject.tag == "Player")
        {
            if (PlayerParry.isParrying)
            {
                mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                transform.rotation = rotation;

                lifetime = maxLifetime;
                PlayerParry.parrySuccess = true;
                parried = true;
            }
            else {
                playerHealth.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        if (parried && other.gameObject.tag == "Enemy")
        {
            enemyHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        
    }
}
