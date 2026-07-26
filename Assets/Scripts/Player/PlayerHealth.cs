using UnityEngine;
using TMPro;
using System;
using Unity.Cinemachine;

public class PlayerHealth : MonoBehaviour
{
    private int _maxHealth = 6;
    private int _health = 6;
    private float _maxInvincibleTimer = 1.1f, _invincibleTimer = 0; // I-frames

    private SpriteRenderer _spriteRenderer;
    [SerializeField] private HealthCard _healthCardPrefab;
    [SerializeField] private Transform _healthCardsTransform;
    public static event Action<int> OnPlayerChangeHealth;
    public static event Action<GameObject> OnPlayerTakeEnemyDamage;

    private CinemachineImpulseSource _screenshake;

    private PlayerVows _playerVows;

    private void Awake()
    {    
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _screenshake = GetComponent<CinemachineImpulseSource>();
        _playerVows = GetComponent<PlayerVows>();
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

    public void Heal(int heal)
    {
        _health += heal;
        _health = Mathf.Min(_health, _maxHealth);
    }

    public void TakeEnemyDamage(GameObject enemy, int damage)
    {
        if (_invincibleTimer > 0) return;

        // Debug.Log("Took " + damage + " damage! Health Left: " + health);
        _health -= damage;
        _invincibleTimer = _maxInvincibleTimer;
        
        OnPlayerTakeEnemyDamage?.Invoke(enemy);
        OnPlayerChangeHealth?.Invoke(_health);
        
        _screenshake?.GenerateImpulse();

        if (_health <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        // Debug.Log("You died!");
        // Destroy(gameObject);
    }
}
