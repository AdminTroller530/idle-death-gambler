using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    float speed = 16f;
    float lifetime = 3f;
    BoxCollider2D col;

    void Awake()
    {
        col = gameObject.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        lifetime -= Time.deltaTime;
        if (lifetime <= 0) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
