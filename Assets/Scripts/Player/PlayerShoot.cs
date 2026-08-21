using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

// using UnityEngine.SceneManagement; // DEBUG

public class PlayerShoot : MonoBehaviour
{
    private Vector2 _mousePos;
    private PlayerBullet _bulletPrefab;
    private bool _isHoldingShoot;

    private PlayerBaseStats _stats;
    private float _shootCooldown = 0;

    private int _initialChipsAmount = 100;

    private PlayerGunVisual _playerGunVisual;
    [SerializeField] private Transform _shootPoint;

    private void Awake()
    {
        _playerGunVisual = GetComponentInChildren<PlayerGunVisual>();
    }

    private void Start()
    {
        _stats = Instantiate(PlayerManager.Instance.BaseStats);
        ChipsManager.Instance.SetChipsAmount(_initialChipsAmount);
    }
    
    public void Shoot(InputAction.CallbackContext context)
    {
        _isHoldingShoot = context.ReadValueAsButton();
    }

    public float GetShootAngle()
    {
        _mousePos = CursorTracker.Pos;
        float angle = Mathf.Atan2(_mousePos.y - _shootPoint.position.y, _mousePos.x - _shootPoint.position.x) * Mathf.Rad2Deg;
        return angle;
    }

    private Quaternion GetFinalShootAngle()
    {
        float angle = GetShootAngle();
        angle += Random.Range(-_stats.ShootInaccuracy, _stats.ShootInaccuracy);
        return Quaternion.Euler(0, 0, angle);
    }

    private void ShootBullet()
    {
        PlayerBullet bullet = PlayerBulletPool.Instance.BulletPool.Get();

        bullet.transform.position = _shootPoint.position;
        bullet.transform.rotation = GetFinalShootAngle();
        bullet.Initialize(_stats);
    }

    private void Update()
    {
        if (_shootCooldown > 0) _shootCooldown -= Time.deltaTime;
        if (_isHoldingShoot && _shootCooldown <= 0 && ChipsManager.Instance.GetChipsAmount() >= _stats.ChipsPerShot && !PlayerParry.IsParrying)
        {
            ShootBullet();
            
            _shootCooldown = _stats.ShootCooldown;
            ChipsManager.Instance.DecreaseChips(_stats.ChipsPerShot);
        }
    }
}
