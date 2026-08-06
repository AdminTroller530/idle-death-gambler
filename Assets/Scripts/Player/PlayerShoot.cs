using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

// using UnityEngine.SceneManagement; // DEBUG

public class PlayerShoot : MonoBehaviour
{
    private Vector2 _mousePos;
    private PlayerBullet _bulletPrefab;
    private bool _isHoldingShoot;

    private float _shootCooldown = 0;
    private float _shootCooldownMax;
    private float _shootInaccuracyMax; // in DEGREES

    private float _bulletSpeed;
    private float _bulletLifetime;
    private float _bulletKnockback;

    private int _initialChipsAmount = 100;
    private int _chipsPerShot = 5;

    private PlayerGunVisual _playerGunVisual;
    [SerializeField] private Transform _shootPoint;

    private void Awake()
    {
        _playerGunVisual = GetComponentInChildren<PlayerGunVisual>();
    }

    private void Start()
    {
        ChipsManager.Instance.SetChipsAmount(_initialChipsAmount);
        SetStats(PlayerManager.Instance.BaseStats);
    }

    private void SetStats(PlayerBaseStats stats)
    {
        _chipsPerShot = stats.ChipsPerShot;
        _bulletSpeed = stats.BulletSpeed;
        _bulletLifetime = stats.BulletLifetime;
        _bulletKnockback = stats.BulletKnockback;
        _shootCooldownMax = stats.ShootCooldown;
        _shootInaccuracyMax = stats.ShootInaccuracy;
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
        angle += Random.Range(-_shootInaccuracyMax, _shootInaccuracyMax);
        return Quaternion.Euler(0, 0, angle);
    }

    private void ShootBullet()
    {
        PlayerBullet bullet = PlayerBulletPool.Instance.BulletPool.Get();

        bullet.transform.position = _shootPoint.position;
        bullet.transform.rotation = GetFinalShootAngle();
        bullet.Initialize(_bulletSpeed, _bulletLifetime, _bulletKnockback, _chipsPerShot, 0f);
    }

    private void Update()
    {
        if (_shootCooldown > 0) _shootCooldown -= Time.deltaTime;
        if (ChipsManager.Instance.GetChipsAmount() >= _chipsPerShot && _isHoldingShoot && _shootCooldown <= 0 && !PlayerParry.IsParrying)
        {
            ShootBullet();
            
            _shootCooldown = _shootCooldownMax;
            ChipsManager.Instance.DecreaseChips(_chipsPerShot);
        }
    }
}
