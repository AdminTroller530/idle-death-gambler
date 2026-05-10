using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameScene : MonoBehaviour
{
    [SerializeField] BlackScreen blackScreen;

    public void Load()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Game");
        operation.allowSceneActivation = false;
        
        yield return StartCoroutine(blackScreen.FadeIn());
        yield return new WaitForSeconds(0.5f);

        while (operation.progress < 0.9f) yield return null;
        operation.allowSceneActivation = true;
        while (!operation.isDone) yield return null;
        blackScreen.StartFadeOut();
    }
}
