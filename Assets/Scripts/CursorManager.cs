using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    [SerializeField] Texture2D texture;
    Vector2 clickPosition;
    [SerializeField] Transform pointer;
    Vector2 pointerPos;
    [SerializeField] Transform player;
    float deadzone = 12f;

    void Awake()
    {
        clickPosition = new Vector2(texture.width * 0.5f, texture.height * 0.5f);
        Cursor.SetCursor(texture, clickPosition, CursorMode.Auto);
    }

    void Update()
    {
        pointerPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 relPos = pointerPos - new Vector2(player.position.x, player.position.y);
        if (relPos.magnitude < deadzone) relPos *= relPos.magnitude / deadzone;

        pointer.position = relPos + new Vector2(player.position.x, player.position.y);
    }
}
