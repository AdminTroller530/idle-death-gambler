using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Rendering.Universal;

public abstract class EnemyHealth : MonoBehaviour
{
    protected EnemyStats _stats;
    protected EnemyBase _enemyBase;

    protected float _health;
    [SerializeField] protected TextMeshProUGUI _healthText; //temp

    private SpriteRenderer _spriteRenderer;
    private Light2D _light;
    private const float DEFAULT_LIGHT_INTENSITY = 0.5f;
    private const float LIGHT_INTENSITY_DROP_SPEED = 0.5f;
    private Material _defaultMaterial;
    private Material _damageFlashMaterial;
    private const float DAMAGE_FLASH_TIME = 0.05f;

    private float _fadeOutValue = 10;
    private const float FADE_OUT_SPEED = 12;
    private Animator _animator;

    protected virtual void Awake()
    {
        _enemyBase = GetComponent<EnemyBase>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _light = GetComponent<Light2D>();
        _animator = GetComponent<Animator>();
    }

    protected virtual void OnEnable()
    {
        _light.intensity = DEFAULT_LIGHT_INTENSITY;
    }

    protected virtual void Start()
    {
        _stats = _enemyBase.Stats;
        _health = _stats.MaxHealth;
        _defaultMaterial = _enemyBase.DefaultMaterial;
        _damageFlashMaterial = _enemyBase.DamageFlashMaterial;
    }

    protected virtual void Update()
    {
        _healthText.text = ((int)_health).ToString();

        if (_enemyBase.IsDead && _light.intensity > 0) _light.intensity -= LIGHT_INTENSITY_DROP_SPEED * Time.deltaTime;

        if (_enemyBase.JustTookMeleeDamage && !PlayerMelee.IsMeleeing) _enemyBase.JustTookMeleeDamage = false;
    }

    protected virtual IEnumerator DamageFlash()
    {
        _spriteRenderer.material = _damageFlashMaterial;
        yield return new WaitForSeconds(DAMAGE_FLASH_TIME);
        _spriteRenderer.material = _defaultMaterial;
    }

    public virtual void TakeDamage(float damage, bool isMelee = false)
    {
        if (_enemyBase.IsDead) return;

        _health -= damage;
        if (isMelee) _enemyBase.JustTookMeleeDamage = true;
        StartCoroutine(DamageFlash());
        if (_health <= 0) StartCoroutine(Death());
    }

    protected virtual IEnumerator Death()
    {
        _enemyBase.IsDead = true;

        if (_animator.runtimeAnimatorController) _animator.SetTrigger("Die");
        // for (int i = 0; i < _stats.ChipsDropped; i++)
        // {
        //     ChipsManager.Instance.SpawnChip(transform.position, ChipValue.White);
        // }
        ChipsManager.Instance.SpawnChips(transform.position, _stats.ChipsDropped);
        yield return new WaitForSeconds(1);

        while (_fadeOutValue > 1)
        {
            _fadeOutValue -= FADE_OUT_SPEED * Time.deltaTime;
            _spriteRenderer.color = new Color(1, 1, 1, Mathf.Log10(_fadeOutValue));
            yield return null;
        }

        Destroy(gameObject);
    }
}
