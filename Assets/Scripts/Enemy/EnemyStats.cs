using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Header("Identification")]
    public int Id;
    public string Name;
    public string Type;

    [Header("Base Stats")]
    public float MaxHealth;
    public float MoveSpeed;
    public float CollisionDamage;

    [Header("Shooting Stats")]
    public float BulletSpeed;
    public float BulletDamage;
    public float BulletLifetime;
    public float BulletStartOffset;
    public float ShootCooldown;
    public float ShootCooldownOffsetMax;
    public float ShootInaccuracy;

    [Header("Sprites")]
    public Sprite EnemySprites;
    public Sprite[] BulletSprites;
}
