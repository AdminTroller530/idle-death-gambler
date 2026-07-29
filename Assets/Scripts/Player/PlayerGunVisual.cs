using UnityEngine;
using static Direction;

public class PlayerGunVisual : MonoBehaviour
{
    private PlayerShoot _playerShoot;
    private SpriteRenderer _spriteRenderer;
    private float _shootAngle;
    private Direction _gunOrientation = Right;

    [SerializeField] private SpriteRenderer _spriteRendererUI;
    
    private const float GUN_ORIENTATION_DEADZONE = 10f;
    private const float SPRITE_OFFSET = 0.6f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerShoot = GetComponentInParent<PlayerShoot>();
    }

    private void Update()
    {
        _shootAngle = _playerShoot.GetShootAngle();

        if (_shootAngle <= 90 - GUN_ORIENTATION_DEADZONE && _shootAngle >= -(90 - GUN_ORIENTATION_DEADZONE) && _gunOrientation == Left) {
            transform.localPosition = new Vector2(SPRITE_OFFSET, 0);
            transform.localScale = new Vector3(1, 1 ,1);
            _gunOrientation = Right;
        }
        else if (_shootAngle >= 90 + GUN_ORIENTATION_DEADZONE || _shootAngle <= -(90 + GUN_ORIENTATION_DEADZONE) && _gunOrientation == Right) {
            transform.localPosition = new Vector2(-SPRITE_OFFSET, 0);
            transform.localScale = new Vector3(1, -1 ,1);
            _gunOrientation = Left;
        }
        transform.rotation = Quaternion.Euler(0, 0, _shootAngle);
    }

    public void SetSprite(Sprite gunSprite)
    {
        _spriteRenderer.sprite = gunSprite;
        _spriteRendererUI.sprite = gunSprite;
    }
}
