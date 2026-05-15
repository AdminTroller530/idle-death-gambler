using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 move;
    // float speed = 16f;
    float speed = 8f;
    Rigidbody2D rb;
    public static bool inCombat = false;

    Animator anim;
    SpriteRenderer sr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>().normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = move * speed;
        sr.flipX = move.x < 0;
        anim.SetFloat("MoveX", move.x);
        anim.SetFloat("MoveY", move.y);
    }

    void Update()
    {
        speed = inCombat ? 8f : 13f;
    }
}
