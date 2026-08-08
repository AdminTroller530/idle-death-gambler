using System;
using UnityEngine;

public class EnemyBase : MonoBehaviour // maybe could make this parent class of other enemy scripts?
{
    [SerializeField] public EnemyStats Stats;

    [SerializeField] public Material DefaultMaterial;
    [SerializeField] public Material DamageFlashMaterial;

    public event Action OnDeath;
    public bool IsDead = false;

    public void TriggerOnDeath()
    {
        IsDead = true;
        OnDeath?.Invoke();
    }
}
