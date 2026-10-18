using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dialogueText;
    private const float SCROLL_DIALOGUE_COOLDOWN = 0.03f;

    private void Start()
    {
        _dialogueText.maxVisibleCharacters = 0;
        // StartCoroutine(ScrollDialogue("<color=#CCCC00>Look, it's not like I don't want to. It's more like, you know, I don't want to.</color>"));
    }

    private IEnumerator ScrollDialogue(string dialogue)
    {
        _dialogueText.maxVisibleCharacters = 0;
        _dialogueText.text = dialogue;
        _dialogueText.ForceMeshUpdate();
        for (int i = 1; i <= _dialogueText.textInfo.characterCount; i++)
        {
            _dialogueText.maxVisibleCharacters = i;
            char character = _dialogueText.textInfo.characterInfo[i-1].character;

            if (character == ',') yield return new WaitForSeconds(SCROLL_DIALOGUE_COOLDOWN * 3);
            else if (character == '.' || character == '!' || character == '?') yield return new WaitForSeconds(SCROLL_DIALOGUE_COOLDOWN * 5);
            else yield return new WaitForSeconds(SCROLL_DIALOGUE_COOLDOWN);
        }
    }
}