using UnityEngine;
using Pathfinding;

public class EnemyPathfind : MonoBehaviour
{
    AIPath path;
    [SerializeField] Transform player;

    void Awake()
    {
        path = GetComponent<AIPath>();
    }

    void Update()
    {
        path.destination = player.position;
    }
}
