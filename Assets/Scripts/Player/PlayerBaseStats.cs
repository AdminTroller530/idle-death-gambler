using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBaseStats", menuName = "Scriptable Objects/PlayerBaseStats")]
public class PlayerBaseStats : ScriptableObject
{
    [Header("Player Stats")]
    public int MaxHealth;
    public float MovementSpeed;
    public float ParryCooldown;

    [Header("Shooting Stats")]
    public int ChipsPerShot;
    public float BulletSpeed;
    public float BulletLifetime;
    public float BulletKnockback;
    public float ShootCooldown;
    public float ShootInaccuracy;
}
