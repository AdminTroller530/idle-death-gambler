using Unity.Cinemachine;
using UnityEngine;

public class Screenshake : Singleton<Screenshake>
{
    [SerializeField] private CinemachineImpulseSource _screenshake;

    public void Shake(float strength)
    {
        _screenshake?.GenerateImpulse(strength);
    }
}
