using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D _cursorTexture;
    private Vector2 _clickPosition;

    [SerializeField] private Transform _followPointer;
    private Vector2 _mousePos;
    // private float _deadzone = 12f;

    private Transform _playerTransform;

    void Awake()
    {
        _clickPosition = new Vector2(_cursorTexture.width * 0.5f, _cursorTexture.height * 0.5f);
        Cursor.SetCursor(_cursorTexture, _clickPosition, CursorMode.Auto);
    }

    private void Start()
    {
        _playerTransform = PlayerManager.Instance.Transform;
    } 

    void Update()
    {
        _mousePos = CursorTracker.Pos;
        Vector2 relPos = _mousePos - (Vector2)_playerTransform.position;
        // if (relPos.magnitude < _deadzone) relPos *= relPos.magnitude / _deadzone;

        _followPointer.position = relPos + (Vector2)_playerTransform.position;
    }
}
