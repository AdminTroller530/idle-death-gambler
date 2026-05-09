using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioController : MonoBehaviour
{
    static StudioEventEmitter emitter;
    // public static float targetLowPass = 1f;

    void Start()
    {
        emitter = GetComponent<StudioEventEmitter>();
    }

    public static void UpdateLowPass(float target)
    {
        emitter.SetParameter("Low Pass", target);
    }

    // void Update()
    // {
    //     emitter.SetParameter("Low Pass", targetLowPass);
    // }
}
