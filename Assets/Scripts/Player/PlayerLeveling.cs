using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLeveling : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentLevelText;
    [SerializeField] private TextMeshProUGUI _currentXPText;
    [SerializeField] private Slider _XPSlider;
    [SerializeField] private TextMeshProUGUI _skillLevelsText;
    [SerializeField] private SkillLevelSelection _skillLevelSelection;

    private float _currentXP = 0;
    private float _currentXPDisplayAmount = 0;
    private int _nextLevelXP = 10;
    private const float XP_DISPLAY_UPDATE_SPEED = 7f;

    public static event Action OnPlayerLevelUp;

    private PlayerBaseStats _stats;
    private const float BASE_MOVEMENT_SPEED = 8f;

    private void Awake()
    {
        _XPSlider.maxValue = 10;
    }

    private void Start()
    {
        _stats = PlayerManager.Instance.Stats;
        UpdateSkillLevelsText();
    }

    public void IncreaseXP(int amount)
    {
        _currentXP += amount;
    }

    private void ManageXP()
    {
        _currentXPDisplayAmount = Mathf.Lerp(_currentXPDisplayAmount, _currentXP, XP_DISPLAY_UPDATE_SPEED * Time.deltaTime);
        if (Mathf.Abs(_currentXP - _currentXPDisplayAmount) < 0.05f) _currentXPDisplayAmount = _currentXP;

        if (_currentXPDisplayAmount >= _nextLevelXP)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        PlayerManager.Instance.Stats.CurrentLevel++;
        _currentXP -= _nextLevelXP;
        _currentXPDisplayAmount = 0;
        _XPSlider.maxValue = 10; // SET UPDATED MAX VALUE LATER

        OnPlayerLevelUp?.Invoke();
        
        _skillLevelSelection.Open();
    }

    public void LevelUpSkill(int attackIncrement = 0, int speedIncrement = 0, int luckIncrement = 0)
    {
        _stats.AttackLevel += attackIncrement;
        _stats.SpeedLevel += speedIncrement;
        _stats.LuckLevel += luckIncrement;
        UpdateStatsFromSkills();
        UpdateSkillLevelsText();
    }

    private void UpdateStatsFromSkills()
    {
        if (_stats.BulletDamageMultiplier > 99) return; // IF DEBUG MODE IS ON, NO CHANGES

        _stats.BulletDamageMultiplier = CalculateDamageMultiplier(_stats.AttackLevel);
        _stats.MovementSpeed = CalculateMovementSpeed(_stats.SpeedLevel);
        // luck modifications too in the future
    }

    public float CalculateDamageMultiplier(int attackLevel) {return 1 + (attackLevel - 1) * 0.25f;}
    public float CalculateMovementSpeed(int speedLevel) {return BASE_MOVEMENT_SPEED + (speedLevel-1) * 0.5f;}
    public int CalculateLuckPercent(int luckLevel) {return 1 + (luckLevel-1) * 4;}

    private void Update()
    {
        ManageXP();
        UpdateUI();
    }

    private void UpdateUI()
    {
        _currentLevelText.text = $"LV{PlayerManager.Instance.Stats.CurrentLevel}";
        _currentXPText.text = $"XP: {(int)_currentXPDisplayAmount}/{_nextLevelXP}";
        _XPSlider.value = _currentXPDisplayAmount;
    }

    private void UpdateSkillLevelsText()
    {
        _skillLevelsText.text = $"<color=#ea4f36>{_stats.AttackLevel}</color> <color=#30e1b9>{_stats.SpeedLevel}</color> <color=#1ebc73>{_stats.LuckLevel}</color>";
    }
}
