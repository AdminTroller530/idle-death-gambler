using UnityEngine;

public class RoomCardDisplayPrefab : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void SetSprite(RoomCardData card)
    {
        _spriteRenderer.sprite = card.Sprite;
    }
}
