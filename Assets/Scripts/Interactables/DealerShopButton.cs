using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DealerShopButton : MonoBehaviour, IInteractable
{
    private PlayerBaseStats _stats;
    private Light2D _selectedLight;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        if (!DealerShopManager.Instance.IsOpen)
        {
            DealerShopManager.Instance.Open();
        }
        else DealerShopManager.Instance.Close();
        
    }

    public void SetIsSelected(bool set)
    {
        _selectedLight.enabled = set;
    }

    public string GetInteractableText()
    {
        return $"<color=#0000DD>Dealer";
    }

    private void Awake()
    {
        _selectedLight = GetComponent<Light2D>();
    }

    private void Start()
    {
        _stats = PlayerManager.Instance.Stats;
    }
}
