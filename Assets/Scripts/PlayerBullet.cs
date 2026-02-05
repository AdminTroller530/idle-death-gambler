using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    float speed = 16f;
    float lifetime = 3f;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        lifetime -= Time.deltaTime;
        if (lifetime <= 0) Destroy(gameObject);
    }
}
