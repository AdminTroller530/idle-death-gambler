using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private IInteractable _selectedInteractable = null;
    [SerializeField] private TextMeshProUGUI _interactableText;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _selectedInteractable?.Interact();
        }
    }

    public void SetInteractableText(string text)
    {
        _interactableText.text = text;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            interactable.SetIsSelected(true);
            _selectedInteractable = interactable;

            _interactableText.text = interactable.GetInteractableText();
            _interactableText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            interactable.SetIsSelected(false);

            if (interactable == _selectedInteractable) {
                _selectedInteractable = null;
                _interactableText.gameObject.SetActive(false);
            }
        }
    }
}
