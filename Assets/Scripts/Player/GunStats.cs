using UnityEngine;

[CreateAssetMenu(fileName = "GunStats", menuName = "Scriptable Objects/GunStats")]
public class GunStats : ScriptableObject
{
    [Header("Identification")]
    public int Id;
    public string Name;
    public string Type;

    [Header("Stats")]
    public float BulletSpeed;
    public float BulletDamage;
    public float BulletLifetime;
    public float BulletKnockback;
    public float ShootCooldown;
    public float ShootInaccuracy;
    public int MagSize;
    public float ReloadTime;

    [Header("Sprites")]
    public Sprite[] GunSprites;
    public PlayerBullet BulletPrefab;
}
