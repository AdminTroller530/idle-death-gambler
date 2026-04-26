using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    [SerializeField] GameObject enemyBullet;
    GameObject enemyBullets;
    GameObject p;
    EnemyStats stats;
    bool seePlayer = false;
    RaycastHit2D ray;
    [SerializeField] LayerMask wallMask;
    float shootCooldown = 0.5f;

    void Awake()
    {
        p = GetComponent<EnemyBase>().p;
        stats = GetComponent<EnemyBase>().stats;
        enemyBullets = GameObject.Find("EnemyBullets");
    }

    void Shoot(float damage, float speed)
    {
        float angle = Mathf.Atan2(p.transform.position.y - transform.position.y, p.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        angle += Random.Range(-stats.shootInaccuracy, stats.shootInaccuracy);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        
        GameObject bulletObject = Instantiate(enemyBullet, transform.position, rotation, enemyBullets.transform);
        bulletObject.GetComponent<EnemyBullet>().playerHealth = p.GetComponent<PlayerHealth>();

    }
    
    // temporary debug enemy shooting test
    float temp = 0;
    void Update()
    {
        CheckSeePlayer();
        if (shootCooldown > 0) shootCooldown -= Time.deltaTime;
        else shootCooldown = 0;

        if (shootCooldown == 0 && seePlayer)
        {
            Shoot(stats.bulletDamage, stats.bulletSpeed);
            shootCooldown = stats.shootCooldown;
        }
    }

    void CheckSeePlayer()
    {
        Debug.DrawRay(transform.position, p.transform.position-transform.position, Color.red);
        ray = Physics2D.Raycast(transform.position, p.transform.position-transform.position, Vector2.Distance(transform.position, p.transform.position), wallMask);
        seePlayer = !ray.collider;
    }
}
