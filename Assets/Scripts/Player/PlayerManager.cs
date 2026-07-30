using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerHealth Health {get; private set;}
    public PlayerShoot Shoot {get; private set;}
    public Transform Transform {get; private set;}

    protected override void Awake()
    {
        base.Awake();

        Health = GetComponent<PlayerHealth>();
        Shoot = GetComponent<PlayerShoot>();
        Transform = transform;
    }

    public Vector2 GetPos() => transform.position;
}
