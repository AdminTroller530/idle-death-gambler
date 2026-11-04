using System;
using TMPro;
using UnityEngine;

public class PlayerLeveling : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentLevelText;
    [SerializeField] private TextMeshProUGUI _currentXPText;

    private float _currentXP = 0;
    private float _nextLevelXP = 10;

    public static event Action OnPlayerLevelUp;

    private void Awake()
    {
        UpdateUI();
    }

    public void IncreaseXP(int amount)
    {
        _currentXP += amount;

        while (_currentXP >= _nextLevelXP)
        {
            PlayerManager.Instance.Stats.CurrentLevel++;
            _currentXP -= _nextLevelXP;
            OnPlayerLevelUp?.Invoke();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        _currentLevelText.text = $"LV{PlayerManager.Instance.Stats.CurrentLevel}";
        _currentXPText.text = $"XP: {_currentXP}/{_nextLevelXP}";
    }
}
