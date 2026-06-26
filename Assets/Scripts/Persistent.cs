using UnityEngine;

public class Persistent : MonoBehaviour
{
    private static Persistent _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
