using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    private float _maxHealth = 40f;
    private float _health = 40f;
    [SerializeField] private TextMeshProUGUI healthText; // temp
    private SpriteRenderer _spriteRenderer;

    private float _maxInvincibleTimer = 1.1f, _invincibleTimer = 0; // I-frames

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        healthText.text = $"{(int)_health}/{(int)_maxHealth}";

        if (_invincibleTimer > 0) {
            _invincibleTimer -= Time.deltaTime;
            _spriteRenderer.color = new Color(1,1,1,0.6f);
        }
        else _spriteRenderer.color = new Color(1,1,1,1);
    }

    public void Heal(float heal)
    {
        _health += heal;
        _health = Mathf.Min(_health, _maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (_invincibleTimer > 0) return;

        _health -= damage;
        // Debug.Log("Took " + damage + " damage! Health Left: " + health);
        if (_health <= 0)
        {
            Death();
        }
        _invincibleTimer = _maxInvincibleTimer;
    }

    private void Death()
    {
        // Debug.Log("You died!");
        // Destroy(gameObject);
    }
}
