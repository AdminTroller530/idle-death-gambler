using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

// using UnityEngine.SceneManagement; // DEBUG

public class PlayerShoot : MonoBehaviour
{
    private Vector2 _mousePos;
    private PlayerBullet _bulletPrefab;
    private bool _isHoldingShoot;

    [SerializeField] private GunStats[] _guns = new GunStats[3];
    private GunStats _currentGun; // current gun
    private int _gunSlot; // current gun slot selected (0-2, index 0)

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

    private int _initialChipsAmount = 100;
    private int _chipsPerShot = 5;

    private PlayerGunVisual _playerGunVisual;
    [SerializeField] private Transform _shootPoint;

    private void Awake()
    {
        _playerGunVisual = GetComponentInChildren<PlayerGunVisual>();
    }

    private void TempStart() // debug starting guns
    {
        for (int i = 0; i < _guns.Length; i++)
        {
            if (!_guns[i]) continue;
            _gunsAmmo[i] = _guns[i].MagSize;
        }
    }

    private void Start()
    {
        TempStart();
        ChipsManager.SetChipsAmount(_initialChipsAmount);
        EquipGun(0);
        // _gunsAmmo[0] = _magSize;
        // UpdateAmmoText();
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

        _playerGunVisual.SetSprite(_currentGun.GunSprites[0]);
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

            // SceneManager.LoadScene(0); // DEBUG
        }
    }

    private void SwitchToWeaponSlot(int slot)
    {
        EquipGun(slot);
        _isReloading = false;
        _reloadTimer = 0;
        // play sound or whatever else
    }

    public void WeaponSlot1(InputAction.CallbackContext context) {if (context.started) SwitchToWeaponSlot(0);}
    public void WeaponSlot2(InputAction.CallbackContext context) {if (context.started) SwitchToWeaponSlot(1);}
    public void WeaponSlot3(InputAction.CallbackContext context) {if (context.started) SwitchToWeaponSlot(2);}

    public void InstantReload(float percentOfMag)
    {
        _gunsAmmo[_gunSlot] = Mathf.Min(_gunsAmmo[_gunSlot] + (int)(_magSize * percentOfMag), _magSize);
        UpdateAmmoText();
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
        angle += Random.Range(-_shootInaccuracy, _shootInaccuracy);
        return Quaternion.Euler(0, 0, angle);
    }

    private void ShootBullet()
    {
        PlayerBullet bullet = PlayerBulletPool.Instance.BulletPool.Get();

        bullet.transform.position = _shootPoint.position;
        bullet.transform.rotation = GetFinalShootAngle();
        bullet.Initialize(_bulletSpeed, _bulletLifetime, _bulletKnockback, _bulletDamage, 0f);
    }

    void Update()
    {
        if (_shootCooldown > 0) _shootCooldown -= Time.deltaTime;
        if (_currentGun && ChipsManager.GetChipsAmount() >= _chipsPerShot && _isHoldingShoot && _shootCooldown <= 0 && !_isReloading && !PlayerParry.IsParrying)
        {
            ShootBullet();
            
            _shootCooldown = _shootCooldownMax;
            _gunsAmmo[_gunSlot] -= 1;
            ChipsManager.DecreaseChips(_chipsPerShot);
            UpdateAmmoText();
        }

        if (_reloadTimer > 0) _reloadTimer -= Time.deltaTime;
        else if (_isReloading)
        {
            _isReloading = false;
            _gunsAmmo[_gunSlot] = _magSize;
            ChipsManager.SetChipsAmount(_initialChipsAmount);
            UpdateAmmoText();
        }
    }
}
