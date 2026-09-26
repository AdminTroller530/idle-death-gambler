using System.Collections;
using UnityEngine;

public class WarpPortal : MonoBehaviour
{
    private bool isUsed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isUsed) return;

        if (other.tag == "Player")
        {
            StartCoroutine(WarpSequence());
            isUsed = true;
        }
    }

    private IEnumerator WarpSequence()
    {
        yield return StartCoroutine(BlackScreen.Instance.FadeIn());
        yield return new WaitForSeconds(0.5f);

        RoomGenerator.Instance.DeleteActiveRooms();
        
        BlackScreen.Instance.StartFadeOut();
    }
}
