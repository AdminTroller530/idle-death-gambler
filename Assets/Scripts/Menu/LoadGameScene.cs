using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameScene : MonoBehaviour
{
    private BlackScreen _blackScreen;

    private void Start()
    {
        _blackScreen = BlackScreen.Instance;
    }

    public void Load() => StartCoroutine(LoadGame());

    private IEnumerator LoadGame()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Game");
        operation.allowSceneActivation = false;
        
        yield return StartCoroutine(_blackScreen.FadeIn());
        yield return new WaitForSeconds(0.2f);

        while (operation.progress < 0.9f) yield return null;
        operation.allowSceneActivation = true;
        while (!operation.isDone) yield return null;

        AstarPath.active.Scan();

        yield return null;
        _blackScreen.StartFadeOut();
    }
}
