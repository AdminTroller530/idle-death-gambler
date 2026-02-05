using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] Texture2D texture;
    Vector2 clickPosition;

    void Awake()
    {
        clickPosition = new Vector2(texture.width * 0.5f, texture.height * 0.5f);
        Cursor.SetCursor(texture, clickPosition, CursorMode.Auto);
    }
}
