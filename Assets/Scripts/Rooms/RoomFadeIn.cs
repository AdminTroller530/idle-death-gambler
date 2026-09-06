using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomFadeIn : MonoBehaviour
{
    // [SerializeField] private bool _willFadeIn = true;
    [SerializeField] private Tilemap[] _tilemaps;
    private const float FADE_SPEED = 0.5f;
    private float _fadeValue = 0f;

    private void Awake()
    {
        foreach (Tilemap tilemap in _tilemaps)
        {
            tilemap.color = new Color(255, 255, 255, 0);
        }
    }

    public IEnumerator FadeIn()
    {
        while (_fadeValue < 1)
        {
            _fadeValue += FADE_SPEED * Time.deltaTime;
            foreach (Tilemap tilemap in _tilemaps)
            {
                tilemap.color += new Color(0, 0, 0, Mathf.Pow(_fadeValue, 2));
            }
            yield return null;
        }
    }
}
