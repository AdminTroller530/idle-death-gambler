using UnityEngine;
using UnityEngine.Rendering.Universal;

public class IncreaseChipsPerShotButton : MonoBehaviour, IInteractable
{
    private PlayerBaseStats _stats;
    private Light2D _selectedLight;
    private static int _cost;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        if (ChipsManager.Instance.GetChipsAmount() < _cost)
        {
            
        }
        else {
            _stats.ChipsPerShot++;
            ChipsManager.Instance.DecreaseChips(_cost);
            UpdateCost();
            ChipsManager.Instance.UpdateChipsPerShotText();
        }

        PlayerManager.Instance.Interact.SetInteractableText(GetInteractableText());
    }

    public void SetIsSelected(bool set)
    {
        _selectedLight.enabled = set;
    }

    private void UpdateCost()
    {
        _cost = _stats.ChipsPerShot * 5;
    }

    public string GetInteractableText()
    {
        return $"<color=#FFFFFF>Cost: <color=#FFFF00>{_cost}";
    }

    private void Awake()
    {
        _selectedLight = GetComponent<Light2D>();
    }

    private void Start()
    {
        _stats = PlayerManager.Instance.Stats;
        UpdateCost();
    }
}
