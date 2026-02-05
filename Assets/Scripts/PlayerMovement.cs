using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 move;
    float speed = 10f;
    Rigidbody2D rb;

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
}
