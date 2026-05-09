using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    float maxHealth = 40f;
    float health = 40f;
    [SerializeField] TextMeshProUGUI healthText; // temp
    SpriteRenderer spriteRenderer;

    float maxInvincibleTimer = 1.1f, invincibleTimer = 0; // I-frames

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        healthText.text = $"{(int)health}/{(int)maxHealth}";

        if (invincibleTimer > 0) {
            invincibleTimer -= Time.deltaTime;
            spriteRenderer.color = new Color(1,1,1,0.6f);
        }
        else spriteRenderer.color = new Color(1,1,1,1);
    }

    public void Heal(float heal)
    {
        health += heal;
        health = Mathf.Min(health, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (invincibleTimer > 0) return;

        health -= damage;
        // Debug.Log("Took " + damage + " damage! Health Left: " + health);
        if (health <= 0)
        {
            Death();
        }
        invincibleTimer = maxInvincibleTimer;
    }

    void Death()
    {
        // Debug.Log("You died!");
        // Destroy(gameObject);
    }
}
