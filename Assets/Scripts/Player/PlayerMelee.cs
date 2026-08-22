using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMelee : MonoBehaviour
{
    private PlayerBaseStats _stats;
    private SpriteRenderer _spriteRenderer;

    private bool _isHoldingMeleeButton = false;
    private float _meleeTimer = 0;
    private const float MELEE_TIMER_MAX = 0.13f;
    private float _meleeCooldown = 0;

    public static bool IsMeleeing = false;

    private float GetMeleeAngle() => Mathf.Atan2(CursorTracker.Pos.y - transform.position.y, CursorTracker.Pos.x - transform.position.x) * Mathf.Rad2Deg;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _stats = Instantiate(PlayerManager.Instance.BaseStats);
    }

    private void Update()
    {
        if (_meleeCooldown > 0) _meleeCooldown -= Time.deltaTime;
        if (_meleeTimer > 0) _meleeTimer -= Time.deltaTime;
        else IsMeleeing = false;

        MeleeAttack();
        _spriteRenderer.enabled = IsMeleeing;
    }

    private void MeleeAttack()
    {
        if (!_isHoldingMeleeButton || _meleeCooldown > 0) return;

        transform.rotation = Quaternion.Euler(0, 0, GetMeleeAngle());

        IsMeleeing = true;
        _meleeTimer = MELEE_TIMER_MAX;
        _meleeCooldown = _stats.MeleeCooldown;
    }

    public void MeleeButtonUpdate(InputAction.CallbackContext context)
    {
        _isHoldingMeleeButton = context.ReadValueAsButton();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!IsMeleeing) return;

        if (other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<EnemyBase>().JustTookMeleeDamage) return;

            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(_stats.MeleeDamage, true);
            other.gameObject.GetComponent<EnemyMove>().TakeKnockback(transform.rotation * Vector2.right, _stats.MeleeKnockback);
        }
    }
}
