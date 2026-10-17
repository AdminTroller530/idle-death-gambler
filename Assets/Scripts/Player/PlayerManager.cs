using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerHealth Health {get; private set;}
    public PlayerMovement Movement {get; private set;}
    public PlayerShoot Shoot {get; private set;}
    public PlayerInteract Interact {get; private set;}
    public Transform Transform {get; private set;}

    [SerializeField] public PlayerBaseStats BaseStats;
    public PlayerBaseStats Stats;

    protected override void Awake()
    {
        base.Awake();

        Health = GetComponent<PlayerHealth>();
        Movement = GetComponent<PlayerMovement>();
        Shoot = GetComponent<PlayerShoot>();
        Interact = GetComponentInChildren<PlayerInteract>();
        Transform = transform;
        Stats = Instantiate(BaseStats);
    }

    public Vector2 GetPos() => transform.position;
}
