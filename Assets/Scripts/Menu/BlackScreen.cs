using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    public static BlackScreen Instance {get; private set;}

    private Image _blackScreen;
    private float _fadeSpeed = 1.8f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _blackScreen = GetComponent<Image>();
    }

    public IEnumerator FadeIn()
    {
        while (_blackScreen.color.a < 1)
        {
            _blackScreen.color += new Color(0, 0, 0, _fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void StartFadeOut() => StartCoroutine(FadeOut());

    public IEnumerator FadeOut()
    {
        while (_blackScreen.color.a > 0)
        {
            _blackScreen.color -= new Color(0, 0, 0, _fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
