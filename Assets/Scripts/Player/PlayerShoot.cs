using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerShoot : MonoBehaviour
{
    Vector2 mousePos;
    PlayerBullet bPrefab;
    bool shootHeld;
    [SerializeField] Transform playerBullets;

    [SerializeField] GunStats[] guns;
    GunStats gun; // current gun
    int gunSlot; // current gun slot selected (1-3)

    float shootCooldown = 0;
    // shootInaccuracy: maximum inaccuracy in DEGREES
    float shootCooldownMax, shootInaccuracy;
    float bSpeed, bLifetime, bKnockback, bDamage;

    int magSize;
    int[] ammo = new int[3];
    bool isReloading = false;
    float reloadTime, reloadTimer = 0;
    [SerializeField] TextMeshProUGUI ammoText;

    void Start()
    {
        EquipGun(0);
        ammo[0] = magSize;
        UpdateAmmoText();
    }

    void UpdateAmmoText()
    {
        ammoText.text = $"{ammo[gunSlot]}/{magSize}";
    }

    void EquipGun(int slot)
    {
        if (!guns[slot]) return;
        
        gun = guns[slot];
        gunSlot = slot;

        shootCooldownMax = gun.ShootCooldown;
        shootInaccuracy = gun.ShootInaccuracy;
        
        bSpeed = gun.BulletSpeed;
        bLifetime = gun.BulletLifetime;
        bKnockback = gun.BulletKnockback;
        bDamage = gun.BulletDamage;

        reloadTime = gun.ReloadTime;
        magSize = gun.MagSize;
        UpdateAmmoText();

        bPrefab = gun.BulletPrefab;
    }
    
    public void Shoot(InputAction.CallbackContext context)
    {
        shootHeld = context.ReadValueAsButton();
    }

    public void Reload(InputAction.CallbackContext context)
    {
        if (context.started && reloadTimer <= 0 && ammo[gunSlot] < magSize)
        {
            isReloading = true;
            reloadTimer = reloadTime;
        }
    }

    void Update()
    {
        if (shootCooldown > 0) shootCooldown -= Time.deltaTime;
        if (gun && ammo[gunSlot] > 0 && shootHeld && shootCooldown <= 0 && !isReloading && !PlayerParry.IsParrying)
        {
            ShootBullet();
            shootCooldown = shootCooldownMax;
        }

        if (reloadTimer > 0) reloadTimer -= Time.deltaTime;
        else if (isReloading)
        {
            isReloading = false;
            ammo[gunSlot] = magSize;
            UpdateAmmoText();
        }
    }

    void ShootBullet()
    {
        mousePos = CursorTracker.Pos;
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        angle += Random.Range(-shootInaccuracy, shootInaccuracy);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        
        PlayerBullet bullet = Instantiate(bPrefab, transform.position, rotation, playerBullets);
        bullet.Initialize(bSpeed, bLifetime, bKnockback, bDamage);

        ammo[gunSlot] -= 1;
        UpdateAmmoText();
    }

}
