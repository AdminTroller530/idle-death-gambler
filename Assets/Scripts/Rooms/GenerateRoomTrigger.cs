using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GenerateRoomTrigger : MonoBehaviour
{
    private BoxCollider2D _triggerZone;
    private bool _isTriggered = false;

    public static event Action OnGenerateNewRoom;

    private void Awake()
    {
        _triggerZone = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isTriggered) return;
        if (other.tag == "Player")
        {
            _isTriggered = true;
            if (RoomsDeckManager.Instance.GetCurrentDeckIndex() >= RoomsDeckManager.Instance.GetDeckSize()) return;
            
            RoomsDeckManager.Instance.RoomDeckEnterAnimation();
            OnGenerateNewRoom?.Invoke();
        }
    }
}
