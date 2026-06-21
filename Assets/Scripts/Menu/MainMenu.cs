using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private LoadGameScene _loadGameScene;
    [SerializeField] private Button _newGameButton, _continueButton, _quitButton;

    private void DisableButtons()
    {
        _newGameButton.interactable = false;
        _continueButton.interactable = false;
        _quitButton.interactable = false;
    }

    public void NewGame()
    {
        DisableButtons();
        _loadGameScene.Load();
    }

    public void Continue() // NO FUNCTIONALITY YET
    {
        DisableButtons();
        _loadGameScene.Load();
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
