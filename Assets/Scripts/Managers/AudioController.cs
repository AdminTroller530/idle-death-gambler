using UnityEngine;
using FMODUnity;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance {get; private set;}

    public StudioEventEmitter _emitter;
    // public static float targetLowPass = 1f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
