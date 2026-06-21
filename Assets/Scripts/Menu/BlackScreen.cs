using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    public static BlackScreen Instance {get; private set;}

    private Image _blackScreen;
    private float _fadeSpeed = 15f;
    private float _fadeValue = 1f; // from 1 - 10

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _blackScreen = GetComponent<Image>();
    }

    public IEnumerator FadeIn()
    {
        while (_fadeValue < 10)
        {
            _fadeValue += _fadeSpeed * Time.deltaTime;
            _blackScreen.color = new Color(0, 0, 0, Mathf.Log10(_fadeValue));
            yield return null;
        }
        _fadeValue = 10;
    }

    public void StartFadeOut() => StartCoroutine(FadeOut());

    public IEnumerator FadeOut()
    {
        while (_fadeValue > 1)
        {
            _fadeValue -= _fadeSpeed * Time.deltaTime;
            _blackScreen.color = new Color(0, 0, 0, Mathf.Log10(_fadeValue));
            yield return null;
        }
        _fadeValue = 1;
    }
}
