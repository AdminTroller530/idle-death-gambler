using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _move, _moveAnim;
    private string _dirPriority = "hor";
    private Vector2 _dir = Vector2.right;
    private float _speed = 8f;
    private Rigidbody2D _rb;
    public static bool InCombat = false;

    private Animator _anim;
    private SpriteRenderer _sr;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>().normalized;
    }

    private void Animate()
    {
        _moveAnim = _move;
        if (_moveAnim == Vector2.right || _moveAnim == Vector2.left) _dirPriority = "hor";
        else if (_moveAnim == Vector2.up || _moveAnim == Vector2.down) _dirPriority = "ver";

        if (_dirPriority == "hor") _moveAnim.y = 0;
        else if (_dirPriority == "ver") _moveAnim.x = 0;

        if (_moveAnim != Vector2.zero) _dir = _moveAnim;
        _sr.flipX = _dir.x < 0;

        _anim.SetFloat("MoveX", _moveAnim.x);
        _anim.SetFloat("MoveY", _moveAnim.y);
        _anim.SetFloat("IdleX", _dir.x);
        _anim.SetFloat("IdleY", _dir.y);
        _anim.SetFloat("Velocity", _move.magnitude);
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _move * _speed;

        Animate();
    }

    private void Update()
    {
        _speed = InCombat ? 8f : 13f;
    }
}
