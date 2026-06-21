using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    private SpriteRenderer _spriteRenderer;
    
    private float _parryTimerMax = 0.25f;
    private float _parryTimer = 0;
    private float _parryCooldownMax = 0.2f;
    private float _parryCooldown = 0;

    public static bool IsParrying = false;
    public static bool WasParrySuccessful = false; //successful parry = can parry again immediately

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
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

        if (IsParrying) _spriteRenderer.sprite = _sprites[1];
        else _spriteRenderer.sprite = _sprites[0];
    }

    public void Parry(InputAction.CallbackContext context)
    {
        if (context.started && ((_parryCooldown <= 0 && !IsParrying) || WasParrySuccessful))
        {
            IsParrying = true;
            WasParrySuccessful = false;
            _parryTimer = _parryTimerMax;
            _parryCooldown = _parryCooldownMax;
        }
    }
}
