using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    Image blackScreen;
    const float fadeSpeed = 1.8f;

    void Awake()
    {
        blackScreen = GetComponent<Image>();
    }

    public IEnumerator FadeIn()
    {
        while (blackScreen.color.a < 1)
        {
            blackScreen.color += new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void StartFadeOut() {StartCoroutine(FadeOut());}

    public IEnumerator FadeOut()
    {
        while (blackScreen.color.a > 0)
        {
            blackScreen.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
