using TMPro;
using UnityEngine;

public class SkillLevelSelection : MonoBehaviour
{
    [SerializeField] GameObject _skillLevelSelectionObject;
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] TextMeshProUGUI _attackSkillStatsText;
    [SerializeField] TextMeshProUGUI _speedSkillStatsText;
    [SerializeField] TextMeshProUGUI _luckSkillStatsText;

    private PlayerBaseStats _stats;
    private PlayerLeveling _playerLeveling;

    private void Awake()
    {
        _skillLevelSelectionObject.SetActive(false);
    }

    private void Start()
    {
        _stats = PlayerManager.Instance.Stats;
        _playerLeveling = PlayerManager.Instance.Leveling;
    }

    public void Open()
    {
        _skillLevelSelectionObject.SetActive(true);
        PauseManager.SetPaused(true);

        UpdateText();
    }

    private void UpdateText()
    {
        _titleText.text = $"Level {_stats.CurrentLevel}";
        _attackSkillStatsText.text = $"<color=#AE2334>Damage Multiplier\n<color=#C35135>{_playerLeveling.CalculateDamageMultiplier(_stats.AttackLevel)}</color> <color=#AE2334>-></color> <color=#E95531>{_playerLeveling.CalculateDamageMultiplier(_stats.AttackLevel+1)}</color>";
        _speedSkillStatsText.text = $"<color=#AE2334>Movement Speed\n<color=#C35135>{_playerLeveling.CalculateMovementSpeed(_stats.SpeedLevel)}</color> <color=#AE2334>-></color> <color=#E95531>{_playerLeveling.CalculateMovementSpeed(_stats.SpeedLevel+1)}</color>";
        _luckSkillStatsText.text = $"<color=#AE2334>Dealer Luck\n<color=#C35135>{_playerLeveling.CalculateLuckPercent(_stats.LuckLevel)}%</color> <color=#AE2334>-></color> <color=#E95531>{_playerLeveling.CalculateLuckPercent(_stats.LuckLevel+1)}%</color>";
    }

    private void Close()
    {
        _skillLevelSelectionObject.SetActive(false);
        PauseManager.SetPaused(false);
    }

    public void LevelUpSkill(string skill)
    {
        if (skill == "attack") _playerLeveling.LevelUpSkill(1, 0, 0);
        else if (skill == "speed") _playerLeveling.LevelUpSkill(0, 1, 0);
        else if (skill == "luck") _playerLeveling.LevelUpSkill(0, 0, 1);
        Close();
    }
}
