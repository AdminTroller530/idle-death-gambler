using UnityEngine;
using UnityEngine.InputSystem;

public class CursorTracker : MonoBehaviour
{
    public static Vector2 Pos;

    private void Awake()
    {
        Pos = Vector2.zero;
    }

    private void Update()
    {
        Pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}
