public interface IInteractable
{
    void Interact();
    bool CanInteract();
    void SetIsSelected(bool set);
    string GetInteractableText();
}