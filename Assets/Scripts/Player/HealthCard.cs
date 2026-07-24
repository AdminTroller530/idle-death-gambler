using UnityEngine;

public class HealthCard : MonoBehaviour
{
    private int _id;
    private float _bobSpeed = 3f;
    private float _bobAmplitude = 2f;
    private float _bobStartOffset;
    private const float START_Y = 150;

    public void Initialize(int id)
    {
        _id = id;
        transform.localPosition = new Vector2(-290 + _id*30, START_Y);
    }

    private void Awake()
    {
        _bobStartOffset = Random.Range(0, 2*Mathf.PI);
    }

    private void Update()
    {
        transform.localPosition = new Vector2(transform.localPosition.x, START_Y + (_bobAmplitude * Mathf.Sin((Time.time + _bobStartOffset) * _bobSpeed)));
    }
}
