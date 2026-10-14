using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private IInteractable _selectedInteractable = null;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _selectedInteractable?.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            interactable.SetIsSelected(true);
            _selectedInteractable = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            interactable.SetIsSelected(false);

            if (interactable == _selectedInteractable) _selectedInteractable = null;
        }
    }
}
