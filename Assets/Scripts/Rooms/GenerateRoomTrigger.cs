using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GenerateRoomTrigger : MonoBehaviour
{
    private BoxCollider2D _triggerZone;
    private bool _isTriggered = false;

    private void Awake()
    {
        _triggerZone = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isTriggered) return;
        if (other.tag == "Player")
        {
            RoomsDeckManager.Instance.RoomDeckEnterAnimation();
            _isTriggered = true;
        }
    }
}
