using UnityEngine;

public class RoomEnterTrigger : MonoBehaviour
{
    private EnemySpawner _enemySpawner;
    private BoxCollider2D _collider;
    private bool _roomIsEntered = false;

    private void Awake()
    {
        _enemySpawner = GetComponentInParent<EnemySpawner>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_roomIsEntered) return;
        if (other.gameObject.tag != "Player") return;

        if (_collider.bounds.Contains(other.bounds.min) && _collider.bounds.Contains(other.bounds.max))
        {
            _roomIsEntered = true;
            RoomsDeckManager.Instance.RoomDeckExitAnimation();
            _enemySpawner.OnEnterRoom();
        }
    }
}
