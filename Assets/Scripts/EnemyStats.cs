using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Header("Base Stats")]
    public float health;
    public float moveSpeed;
    public float bulletSpeed;
    public float bulletDamage;
    public float shootCooldown;
    
}
