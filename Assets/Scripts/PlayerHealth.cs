using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    float maxHealth = 40f;
    float health = 40f;

    public void Heal(float heal)
    {
        health += heal;
        health = Mathf.Min(health, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Took " + damage + " damage! Health Left: " + health);
        if (health <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Debug.Log("You died!");
        // Destroy(gameObject);
    }
}
