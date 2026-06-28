using UnityEngine;
using UnityEngine.Pool;

public class EnemyBulletPool : MonoBehaviour
{
    public static EnemyBulletPool Instance {get; private set;}

    private const int BULLET_POOL_SIZE = 25;
    public ObjectPool<EnemyBullet> BulletPool;

    [SerializeField] private EnemyBullet _enemyBulletPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    private void InitializeBulletPool()
    {
        EnemyBullet[] initializedBullets = new EnemyBullet[BULLET_POOL_SIZE];
        for (int i = 0; i < BULLET_POOL_SIZE; i++) initializedBullets[i] = BulletPool.Get();
        for (int i = 0; i < BULLET_POOL_SIZE; i++) BulletPool.Release(initializedBullets[i]);
    }

    private void Start()
    {
        BulletPool = new ObjectPool<EnemyBullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, true, BULLET_POOL_SIZE, BULLET_POOL_SIZE*2);
        InitializeBulletPool();
    }

    private EnemyBullet CreateBullet()
    {
        EnemyBullet bullet = Instantiate(_enemyBulletPrefab, Vector2.zero, Quaternion.identity, transform);
        return bullet;
    }

    private void OnGetBullet(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseBullet(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(EnemyBullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}
