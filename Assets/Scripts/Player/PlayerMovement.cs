using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 move, moveAnim;
    string dirPriority = "hor";
    Vector2 dir = Vector2.right;
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

    void Animate()
    {
        moveAnim = move;
        if (moveAnim == Vector2.right || moveAnim == Vector2.left) dirPriority = "hor";
        else if (moveAnim == Vector2.up || moveAnim == Vector2.down) dirPriority = "ver";

        if (dirPriority == "hor") moveAnim.y = 0;
        else if (dirPriority == "ver") moveAnim.x = 0;

        if (moveAnim != Vector2.zero) dir = moveAnim;
        sr.flipX = dir.x < 0;

        anim.SetFloat("MoveX", moveAnim.x);
        anim.SetFloat("MoveY", moveAnim.y);
        anim.SetFloat("IdleX", dir.x);
        anim.SetFloat("IdleY", dir.y);
        anim.SetFloat("Velocity", move.magnitude);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = move * speed;

        Animate();
    }

    void Update()
    {
        speed = inCombat ? 8f : 13f;
    }
}
