using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    float maxHealth = 40f;
    float health = 40f;
    [SerializeField] TextMeshProUGUI healthText; // temp

    void Update()
    {
        healthText.text = ((int)health).ToString();
    }

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
