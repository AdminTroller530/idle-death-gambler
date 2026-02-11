using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    float maxHealth = 20f;
    float health = 20f;

    public void Heal(float heal)
    {
        health += heal;
        health = Mathf.Min(health, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // death
        }
    }
}
