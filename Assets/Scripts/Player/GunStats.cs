using UnityEngine;

[CreateAssetMenu(fileName = "GunStats", menuName = "Scriptable Objects/GunStats")]
public class GunStats : ScriptableObject
{
    [Header("Identification")]
    public int id;
    public string idName;
    public string type;

    [Header("Stats")]
    public float bulletSpeed;
    public float bulletDamage;
    public float bulletLifetime;
    public float bulletKnockback;
    public float shootCooldown;
    public float shootInaccuracy;
    public int magSize;
    public float reloadTime;

    [Header("Sprites")]
    public Sprite[] gunSprites;
    public PlayerBullet bulletPrefab;
}
