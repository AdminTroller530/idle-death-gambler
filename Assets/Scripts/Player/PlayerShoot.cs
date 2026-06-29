using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerShoot : MonoBehaviour
{
    private Vector2 _mousePos;
    private PlayerBullet _bulletPrefab;
    private bool _isHoldingShoot;

    [SerializeField] private GunStats[] _guns;
    private GunStats _currentGun; // current gun
    private int _gunSlot; // current gun slot selected (1-3)

    private float _shootCooldown = 0;
    private float _shootCooldownMax;
    private float _shootInaccuracy; // maximum inaccuracy in DEGREES

    private float _bulletSpeed;
    private float _bulletLifetime;
    private float _bulletKnockback;
    private float _bulletDamage;

    private int _magSize;
    private int[] _gunsAmmo = new int[3];
    private bool _isReloading = false;
    private float _gunReloadTime;
    private float _reloadTimer = 0;
    [SerializeField] private TextMeshProUGUI _ammoText;

    private void Start()
    {
        EquipGun(0);
        _gunsAmmo[0] = _magSize;
        UpdateAmmoText();
    }

    void UpdateAmmoText()
    {
        _ammoText.text = $"{_gunsAmmo[_gunSlot]}/{_magSize}";
    }

    void EquipGun(int slot)
    {
        if (!_guns[slot]) return;
        
        _currentGun = _guns[slot];
        _gunSlot = slot;

        _shootCooldownMax = _currentGun.ShootCooldown;
        _shootInaccuracy = _currentGun.ShootInaccuracy;
        
        _bulletSpeed = _currentGun.BulletSpeed;
        _bulletLifetime = _currentGun.BulletLifetime;
        _bulletKnockback = _currentGun.BulletKnockback;
        _bulletDamage = _currentGun.BulletDamage;

        _gunReloadTime = _currentGun.ReloadTime;
        _magSize = _currentGun.MagSize;
        UpdateAmmoText();

        _bulletPrefab = _currentGun.BulletPrefab;
    }
    
    public void Shoot(InputAction.CallbackContext context)
    {
        _isHoldingShoot = context.ReadValueAsButton();
    }

    public void Reload(InputAction.CallbackContext context)
    {
        if (context.started && _reloadTimer <= 0 && _gunsAmmo[_gunSlot] < _magSize)
        {
            _isReloading = true;
            _reloadTimer = _gunReloadTime;
        }
    }

    private Quaternion GetShootAngle()
    {
        _mousePos = CursorTracker.Pos;
        float angle = Mathf.Atan2(_mousePos.y - transform.position.y, _mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        angle += Random.Range(-_shootInaccuracy, _shootInaccuracy);
        return Quaternion.Euler(0, 0, angle);
    }

    private void ShootBullet()
    {
        PlayerBullet bullet = PlayerBulletPool.Instance.BulletPool.Get();

        bullet.transform.position = transform.position;
        bullet.transform.rotation = GetShootAngle();
        bullet.Initialize(_bulletSpeed, _bulletLifetime, _bulletKnockback, _bulletDamage, 1.3f);
    }

    void Update()
    {
        if (_shootCooldown > 0) _shootCooldown -= Time.deltaTime;
        if (_currentGun && _gunsAmmo[_gunSlot] > 0 && _isHoldingShoot && _shootCooldown <= 0 && !_isReloading && !PlayerParry.IsParrying)
        {
            ShootBullet();
            
            _shootCooldown = _shootCooldownMax;
            _gunsAmmo[_gunSlot] -= 1;
            UpdateAmmoText();
        }

        if (_reloadTimer > 0) _reloadTimer -= Time.deltaTime;
        else if (_isReloading)
        {
            _isReloading = false;
            _gunsAmmo[_gunSlot] = _magSize;
            UpdateAmmoText();
        }
    }
}
