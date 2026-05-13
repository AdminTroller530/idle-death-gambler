using UnityEngine;
using UnityEngine.InputSystem;

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
    float shootCooldownMax, shootInaccuracy, reloadTime;
    int magSize;
    int[] ammo = new int[3];
    float bSpeed, bLifetime, bKnockback, bDamage;

    void Start()
    {
        EquipGun(0);
        ammo[0] = magSize;
    }

    void EquipGun(int slot)
    {
        if (!guns[slot]) return;
        
        gun = guns[slot];
        gunSlot = slot;

        shootCooldownMax = gun.shootCooldown;
        shootInaccuracy = gun.shootInaccuracy;
        
        bSpeed = gun.bulletSpeed;
        bLifetime = gun.bulletLifetime;
        bKnockback = gun.bulletKnockback;
        bDamage = gun.bulletDamage;

        reloadTime = gun.reloadTime;
        magSize = gun.magSize;

        bPrefab = gun.bulletPrefab;
    }
    
    public void Shoot(InputAction.CallbackContext context)
    {
        shootHeld = context.ReadValueAsButton();
    }

    void Update()
    {
        if (shootCooldown > 0) shootCooldown -= Time.deltaTime;
        if (gun && shootHeld && shootCooldown <= 0 && !PlayerParry.isParrying)
        {
            ShootBullet();
            shootCooldown = shootCooldownMax;
        }
    }

    void ShootBullet()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        angle += Random.Range(-shootInaccuracy, shootInaccuracy);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        
        PlayerBullet bullet = Instantiate(bPrefab, transform.position, rotation, playerBullets);
        bullet.Initialize(bSpeed, bLifetime, bKnockback, bDamage);
    }

}
