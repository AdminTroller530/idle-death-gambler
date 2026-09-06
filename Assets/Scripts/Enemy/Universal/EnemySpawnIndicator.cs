using System.Collections;
using UnityEngine;

public class EnemySpawnIndicator : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private const float BLINK_TIME = 0.22f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        yield return new WaitForSeconds(BLINK_TIME);
        _spriteRenderer.enabled = !_spriteRenderer.enabled;
        StartCoroutine(Blink());
    }
}
