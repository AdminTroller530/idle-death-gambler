using UnityEngine;

public class RoomCardDisplayPrefab : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Color NORMAL_COLOR = new Color(1, 1, 1);
    private Color DISCARDED_COLOR = new Color(0.5f, 0.5f, 0.5f);

    public void SetSprite(RoomCardData card)
    {
        _spriteRenderer.sprite = card.Sprite;
    }

    public void SetDiscardedState(bool isDiscarded)
    {
        if (isDiscarded) _spriteRenderer.color = DISCARDED_COLOR;
        else _spriteRenderer.color = NORMAL_COLOR;
    }
}
