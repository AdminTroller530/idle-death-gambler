using UnityEngine;

public class HealthCardAnimation : MonoBehaviour
{

    private float _bobSpeed = 5f;
    private float _bobAmplitude = 2f;
    private float _bobStartOffset;
    private float _startY;

    private void Awake()
    {
        _startY = transform.localPosition.y;
        _bobStartOffset = Random.Range(0, 2*Mathf.PI);
    }

    private void Update()
    {
        transform.localPosition = new Vector2(transform.localPosition.x, _startY + (_bobAmplitude * Mathf.Sin((Time.time + _bobStartOffset) * _bobSpeed)));
    }
}
