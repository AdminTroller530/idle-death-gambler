using UnityEngine;

public class RoomCardDisplayPrefab : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Color NORMAL_COLOR = new Color(1, 1, 1);
    private Color DISCARDED_COLOR = new Color(0.5f, 0.5f, 0.5f);
    private Color ACTIVE_COLOR = new Color(0.2f, 0.6f, 0.2f);

    public void SetSprite(RoomCardData card)
    {
        _spriteRenderer.sprite = card.Sprite;
    }

    public void SetState(string state)
    {
        if (state == "discarded") _spriteRenderer.color = DISCARDED_COLOR;
        else if (state == "active") _spriteRenderer.color = ACTIVE_COLOR;
        else _spriteRenderer.color = NORMAL_COLOR;
    }
}
