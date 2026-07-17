using UnityEngine;

public class PlayerGunVisual : MonoBehaviour
{
    private PlayerShoot _playerShoot;
    private SpriteRenderer _spriteRenderer;
    private float _shootAngle;

    private const float SPRITE_OFFSET = 0.5f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerShoot = GetComponentInParent<PlayerShoot>();
    }

    private void Update()
    {
        _shootAngle = _playerShoot.GetShootAngle();

        if (_shootAngle <= 90 && _shootAngle >= -90) { // gun facing right
            transform.localPosition = new Vector2(SPRITE_OFFSET, 0);
            _spriteRenderer.flipY = false;
        }
        else { // gun facing left
            transform.localPosition = new Vector2(-SPRITE_OFFSET, 0);
            _spriteRenderer.flipY = true;
        }
        transform.rotation = Quaternion.Euler(0, 0, _shootAngle);
    }

    public void SetSprite(Sprite gunSprite)
    {
        _spriteRenderer.sprite = gunSprite;
    }
}
