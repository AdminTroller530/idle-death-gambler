using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Header("Identification")]
    public int Id;
    public string Name;

    [Header("Base Stats")]
    public float MaxHealth;
    public float MoveSpeed;
    public float PreferredDistanceFromPlayer;
    public int ChipsDropped;
    public int CollisionDamage = 1;

    [Header("Shooting Stats")]
    public float BulletSpeed;
    public int BulletDamage = 1;
    public float BulletLifetime;
    public float BulletStartOffset;
    public Vector2 BulletHitboxOffset;
    public Vector2 BulletHitboxSize;
    public float BulletWallCollisionCooldown = 0;
    public bool BulletHasWallDestroyParticles = true;
    public float ShootCooldown;
    public float ShootCooldownOffsetMax;
    public float ShootInaccuracy = 0;

    [Header("Sprites")]
    public Sprite EnemySprites;
    public Sprite[] BulletSprites;
}
