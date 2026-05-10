using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameScene : MonoBehaviour
{
    [SerializeField] BlackScreen blackScreen;

    void Start()
    {
        StartCoroutine(Load());
    }

    public IEnumerator Load()
    {
        yield return new WaitForSeconds(3f);
        AsyncOperation operation = SceneManager.LoadSceneAsync("Game");
        operation.allowSceneActivation = false;
        
        yield return StartCoroutine(blackScreen.FadeIn());
        yield return new WaitForSeconds(0.5f);

        while (operation.progress < 0.9f)
        {
            // Debug.Log(operation.progress);
            yield return null;
        }
        blackScreen.StartFadeOut();
        operation.allowSceneActivation = true;
    }
}
