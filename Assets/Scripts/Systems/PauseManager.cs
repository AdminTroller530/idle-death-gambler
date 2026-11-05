using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static bool IsPaused {private set; get;}

    public static void SetPaused(bool condition)
    {
        IsPaused = condition;

        if (IsPaused) Time.timeScale = 0;
        else Time.timeScale = 1;
    }
}
