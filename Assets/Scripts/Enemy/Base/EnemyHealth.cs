using UnityEngine;
using TMPro;
using System.Collections;

public abstract class EnemyHealth : MonoBehaviour
{
    protected EnemyStats _stats;
    protected EnemyBase _enemyBase;

    protected float _health;
    [SerializeField] protected TextMeshProUGUI _healthText; //temp

    private SpriteRenderer _spriteRenderer;
    private Material _defaultMaterial;
    private Material _damageFlashMaterial;
    private const float DAMAGE_FLASH_TIME = 0.05f;

    private Animator _animator;

    protected virtual void Awake()
    {
        _enemyBase = GetComponent<EnemyBase>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        _stats = _enemyBase.Stats;
        _defaultMaterial = _enemyBase.DefaultMaterial;
        _damageFlashMaterial = _enemyBase.DamageFlashMaterial;
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
        if (_enemyBase.IsDead) return;

        _health -= damage;
        StartCoroutine(DamageFlash());
        if (_health <= 0) StartCoroutine(Death());
    }

    protected virtual IEnumerator Death()
    {
        _enemyBase.IsDead = true;

        if (_animator.runtimeAnimatorController) _animator.SetTrigger("Die");
        for (int i = 0; i < _stats.ChipsDropped; i++)
        {
            ChipsManager.Instance.SpawnChip(transform.position, 1);
        }

        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
