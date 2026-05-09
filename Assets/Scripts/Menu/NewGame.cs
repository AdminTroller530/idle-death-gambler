using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        // yield return new WaitForSeconds(3);

        AsyncOperation operation = SceneManager.LoadSceneAsync("Game");
        while (!operation.isDone)
        {
            // Debug.Log(operation.progress);
            yield return null;
        }
    }
}
