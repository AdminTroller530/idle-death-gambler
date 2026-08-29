using UnityEngine;

public class RoomEnterTrigger : MonoBehaviour
{
    private EnemySpawner _enemySpawner;
    private BoxCollider2D _collider;

    private void Awake()
    {
        _enemySpawner = GetComponentInParent<EnemySpawner>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player") return;

        if (_collider.bounds.Contains(other.bounds.min) && _collider.bounds.Contains(other.bounds.max))
        {
            _enemySpawner.OnEnterRoom();
        }
    }
}
