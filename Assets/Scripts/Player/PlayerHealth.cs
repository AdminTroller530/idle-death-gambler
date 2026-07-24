using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    private float _maxHealth = 5f;
    private float _health = 5f;
    private float _maxInvincibleTimer = 1.1f, _invincibleTimer = 0; // I-frames

    private SpriteRenderer _spriteRenderer;
    // ADD event action for take damage to broadcast to healthcards
    [SerializeField] private HealthCard _healthCardPrefab;
    [SerializeField] private Transform _healthCardsTransform;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        CreateHealthCards();
    }

    private void CreateHealthCards()
    {
        for (int id = 0; id < _health; id++)
        {
            HealthCard healthCard = Instantiate(_healthCardPrefab, new Vector2(0, 0), transform.rotation, _healthCardsTransform);
            healthCard.Initialize(id);
        }
    }

    private void Update()
    {
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
