using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : MonoBehaviour
{
    private Animator _animator;
    private ParticleSystem _parryParticles;
    
    private float _parryTimerMax = 0.25f; // how long the parry state stays effective
    private float _parryTimer = 0;
    private float _parryCooldownMax; // how long before player can parry again after a parry ends
    private float _parryCooldown = 0;

    public static bool IsParrying = false;
    public static bool WasParrySuccessful = false; //successful parry = can parry again immediately

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _parryParticles = GetComponent<ParticleSystem>();
    }

    void Start()
    {
        _parryCooldownMax = PlayerManager.Instance.BaseStats.ParryCooldown;
    }

    void Update()
    {
        if (_parryTimer > 0)
        {
            _parryTimer -= Time.deltaTime;
        }
        else
        {
            if (_parryCooldown > 0) _parryCooldown -= Time.deltaTime;
            else _parryCooldown = 0;
            _parryTimer = 0;
            IsParrying = false;
        }

        _animator.SetBool("IsParrying", IsParrying);
    }

    public void Parry(InputAction.CallbackContext context)
    {
        if (context.started && ((_parryCooldown <= 0 && !IsParrying) || WasParrySuccessful))
        {
            IsParrying = true;
            WasParrySuccessful = false;
            _parryTimer = _parryTimerMax;
            _parryCooldown = _parryCooldownMax;
            _parryParticles.Play();
        }
    }
}
