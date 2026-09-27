using System.Collections;
using UnityEngine;

public class WarpPortal : MonoBehaviour
{
    public static bool IsWarping {private set; get;} = false;

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
        yield return new WaitForSeconds(0.5f);

        RoomGenerator.Instance.DeleteActiveRooms();
        IsWarping = false;

        BlackScreen.Instance.StartFadeOut();
    }
}
