using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GenerateRoomTrigger : MonoBehaviour
{
    private BoxCollider2D _triggerZone;
    private bool isTriggered = false;

    private void Awake()
    {
        _triggerZone = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered) return;
        if (other.tag == "Player")
        {
            StartCoroutine(RoomsDeckManager.Instance.InitializeNextRoom());
            isTriggered = true;
        }
    }
}
