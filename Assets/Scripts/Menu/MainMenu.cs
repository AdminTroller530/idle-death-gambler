using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] LoadGameScene loadGameScene;
    [SerializeField] Button newGameButton, continueButton, quitButton;

    void DisableButtons()
    {
        newGameButton.interactable = false;
        continueButton.interactable = false;
        quitButton.interactable = false;
    }

    public void NewGame()
    {
        DisableButtons();
        loadGameScene.Load();
    }

    public void Continue() // NO FUNCTIONALITY YET
    {
        DisableButtons();
        loadGameScene.Load();
    }

    public void Quit()
    {
        DisableButtons();
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
