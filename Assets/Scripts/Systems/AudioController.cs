using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioController : Singleton<AudioController>
{
    [SerializeField] private EventReference _musicReference;
    private EventInstance _musicEventInstance;

    protected override void Awake()
    {
        base.Awake();
        _musicEventInstance = RuntimeManager.CreateInstance(_musicReference);
    }

    private void Start()
    {
        _musicEventInstance.start();
    }

    public void UpdateLowPass(float target)
    {
        RuntimeManager.StudioSystem.setParameterByName("Low Pass", target);
    }
}
