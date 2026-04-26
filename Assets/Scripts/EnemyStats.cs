using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Header("Identification")]
    public int id;
    public string idName;
    public string type;

    [Header("Base Stats")]
    public float maxHealth;
    public float moveSpeed;
    public float collisionDamage;
    public float bulletSpeed;
    public float bulletDamage;
    public float shootCooldown;
    public float shootCooldownOffsetMax;
    public float shootInaccuracy;
}
