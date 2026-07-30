using UnityEngine;
using FMODUnity;

public class AudioController : Singleton<AudioController>
{
    public StudioEventEmitter _emitter;
    // public static float targetLowPass = 1f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _emitter = GetComponent<StudioEventEmitter>();
    }

    public void UpdateLowPass(float target)
    {
        _emitter.SetParameter("Low Pass", target);
    }

    // private void Update()
    // {
    //     _emitter.SetParameter("Low Pass", targetLowPass);
    // }
}
