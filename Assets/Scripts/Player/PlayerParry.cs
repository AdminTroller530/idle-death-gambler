using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // TEMP DEBUG

public class PlayerParry : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    SpriteRenderer s;
    public static bool isParrying = false;
    public static bool parrySuccess = false; //successful parry = can parry immediately again
    float parryTimerMax = 0.25f, parryTimer = 0;
    float parryCooldownMax = 0.2f, parryCooldown = 0;

    void Awake()
    {
        s = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (parryTimer > 0)
        {
            parryTimer -= Time.deltaTime;
        }
        else
        {
            if (parryCooldown > 0) parryCooldown -= Time.deltaTime;
            else parryCooldown = 0;
            parryTimer = 0;
            isParrying = false;
        }

        if (isParrying) s.sprite = sprites[1];
        else s.sprite = sprites[0];
    }

    public void Parry(InputAction.CallbackContext context)
    {
        if (context.started && ((parryCooldown <= 0 && !isParrying) || parrySuccess))
        {
            SceneManager.LoadScene("Main Menu"); // TEMP DEBUG
            isParrying = true;
            parrySuccess = false;
            parryTimer = parryTimerMax;
            parryCooldown = parryCooldownMax;
        }
    }
}
