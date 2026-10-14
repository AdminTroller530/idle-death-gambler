using UnityEngine;
using UnityEngine.Rendering.Universal;

public class IncreaseChipsPerShotButton : MonoBehaviour, IInteractable
{
    private PlayerBaseStats _stats;
    private Light2D _selectedLight;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        _stats.ChipsPerShot++;
    }

    public void SetIsSelected(bool set)
    {
        _selectedLight.enabled = set;
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
