using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 move;
    // float speed = 16f;
    float speed = 8f;
    Rigidbody2D rb;
    public static bool inCombat = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>().normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = move * speed;
    }

    void Update()
    {
        speed = inCombat ? 8f : 13f;
    }
}
