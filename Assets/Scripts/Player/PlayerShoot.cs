using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    Vector2 mousePos;
    [SerializeField] GameObject bPrefab;
    bool shootHeld;

    float shootCooldownMax = 0.35f;
    float shootCooldown = 0;
    float shootInaccuracy = 5f; // maximum inaccuracy in degrees
    
    public void Shoot(InputAction.CallbackContext context)
    {
        shootHeld = context.ReadValueAsButton();
    }

    void Update()
    {
        if (shootCooldown > 0) shootCooldown -= Time.deltaTime;
        if (shootHeld && shootCooldown <= 0 && !PlayerParry.isParrying)
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
        
        GameObject bullet = Instantiate(bPrefab, transform.position, rotation);
        bullet.GetComponent<PlayerBullet>().dir = rotation * Vector2.right;
    }

}
