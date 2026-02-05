using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    Vector2 mousePos;
    [SerializeField] GameObject bPrefab;
    bool shootHeld;
    float shootCooldownMax = 0.35f;
    float shootCooldown = 0;
    
    public void Shoot(InputAction.CallbackContext context)
    {
        shootHeld = context.ReadValueAsButton();

        // if (context.started)
        // {
            
        // }
    }

    void Update()
    {
        if (shootCooldown > 0) shootCooldown -= Time.deltaTime;
        if (shootHeld && shootCooldown <= 0)
        {
            ShootBullet();
            shootCooldown = shootCooldownMax;
        }
    }

    void ShootBullet()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        
        Instantiate(bPrefab, transform.position, rotation);
    }

}
