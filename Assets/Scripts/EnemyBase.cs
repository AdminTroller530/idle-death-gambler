using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] public EnemyStats stats;
    public GameObject p;

    void Awake()
    {
        p = GameObject.Find("Player");
    }
}
