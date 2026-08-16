using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : MonoBehaviour
{
    private Animator _animator;
    private ParticleSystem _parryParticles;
    
    private float _parryTimerMax = 0.25f;
    private float _parryTimer = 0;
    private float _parryCooldownMax = 0.2f;
    private float _parryCooldown = 0;

    public static bool IsParrying = false;
    public static bool WasParrySuccessful = false; //successful parry = can parry again immediately

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _parryParticles = GetComponent<ParticleSystem>();
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
