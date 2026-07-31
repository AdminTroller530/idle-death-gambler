using UnityEngine;
using TMPro;
using System.Collections;

public abstract class EnemyHealth : MonoBehaviour
{
    protected EnemyStats _stats;

    protected float _health;
    [SerializeField] protected TextMeshProUGUI _healthText; //temp

    private SpriteRenderer _spriteRenderer;
    private Material _defaultMaterial;
    private Material _damageFlashMaterial;
    private const float DAMAGE_FLASH_TIME = 0.075f;

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        _stats = GetComponent<EnemyBase>().Stats;
        _defaultMaterial = GetComponent<EnemyBase>().DefaultMaterial;
        _damageFlashMaterial = GetComponent<EnemyBase>().DamageFlashMaterial;
        _health = _stats.MaxHealth;
    }

    protected virtual void Update()
    {
        _healthText.text = ((int)_health).ToString();
    }

    protected virtual IEnumerator DamageFlash()
    {
        _spriteRenderer.material = _damageFlashMaterial;
        yield return new WaitForSeconds(DAMAGE_FLASH_TIME);
        _spriteRenderer.material = _defaultMaterial;
    }

    public virtual void TakeDamage(float damage)
    {
        _health -= damage;
        StartCoroutine(DamageFlash());
        if (_health <= 0) Death();
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }
}
