using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    private EnemyBase _enemyBase;
    private EnemyStats _stats;

    private float _health;
    [SerializeField] private TextMeshProUGUI _healthText; //temp

    private void Awake()
    {
        _enemyBase = GetComponent<EnemyBase>();
    }

    private void Start()
    {
        _stats = _enemyBase.Stats;
        _health = _stats.maxHealth;
    }

    private void Update()
    {
        _healthText.text = ((int)_health).ToString();
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0) Death();
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
