using UnityEngine;
using UnityEngine.Pool;

public class PlayerBulletPool : MonoBehaviour
{
    public static PlayerBulletPool Instance {get; private set;}

    private const int BULLET_POOL_SIZE = 20;
    public ObjectPool<PlayerBullet> BulletPool;

    [SerializeField] private PlayerBullet _playerBulletPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void InitializeBulletPool()
    {
        PlayerBullet[] initializedBullets = new PlayerBullet[BULLET_POOL_SIZE];
        for (int i = 0; i < BULLET_POOL_SIZE; i++) initializedBullets[i] = BulletPool.Get();
        for (int i = 0; i < BULLET_POOL_SIZE; i++) BulletPool.Release(initializedBullets[i]);
    }

    private void Start()
    {
        BulletPool = new ObjectPool<PlayerBullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, true, BULLET_POOL_SIZE, BULLET_POOL_SIZE*2);
        InitializeBulletPool();
    }

    private PlayerBullet CreateBullet()
    {
        PlayerBullet bullet = Instantiate(_playerBulletPrefab, Vector2.zero, Quaternion.identity, transform);
        return bullet;
    }

    private void OnGetBullet(PlayerBullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseBullet(PlayerBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(PlayerBullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}
