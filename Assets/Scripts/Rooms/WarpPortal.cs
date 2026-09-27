using System;
using System.Collections;
using UnityEngine;

public class WarpPortal : MonoBehaviour
{
    public static bool IsWarping {private set; get;} = false;
    public static event Action OnWarpToNextFloor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsWarping) return;

        if (other.tag == "Player")
        {
            StartCoroutine(WarpSequence());
            IsWarping = true;
        }
    }

    private IEnumerator WarpSequence()
    {
        yield return StartCoroutine(BlackScreen.Instance.FadeIn());

        RoomGenerator.Instance.DeleteActiveRooms();
        OnWarpToNextFloor?.Invoke();

        yield return new WaitForSeconds(0.5f);
        IsWarping = false;
        BlackScreen.Instance.StartFadeOut();
    }
}
