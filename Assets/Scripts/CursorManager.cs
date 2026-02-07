using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    [SerializeField] Texture2D texture;
    Vector2 clickPosition;
    [SerializeField] Transform pointer;
    Vector2 pointerPos;
    [SerializeField] Transform player;
    float deadzone = 5f;

    void Awake()
    {
        clickPosition = new Vector2(texture.width * 0.5f, texture.height * 0.5f);
        Cursor.SetCursor(texture, clickPosition, CursorMode.Auto);
    }

    void Update()
    {
        pointerPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 newPos = pointerPos - new Vector2(player.position.x, player.position.y);
        if (newPos.magnitude < deadzone) newPos = player.position;
        else if (newPos.magnitude < deadzone*3) newPos *= (newPos.magnitude - deadzone) / (deadzone*2);

        pointer.position = newPos;
    }
}
