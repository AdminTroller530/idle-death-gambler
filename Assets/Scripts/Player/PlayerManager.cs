using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance {get; private set;}

    public PlayerHealth Health {get; private set;}
    public PlayerShoot Shoot {get; private set;}
    public Transform Transform {get; private set;}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Health = GetComponent<PlayerHealth>();
            Shoot = GetComponent<PlayerShoot>();
            Transform = transform;
        }
        else Destroy(gameObject);
    }

    public Vector2 GetPos() => transform.position;
}
