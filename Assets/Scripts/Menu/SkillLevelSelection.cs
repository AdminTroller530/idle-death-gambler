using UnityEngine;

public class SkillLevelSelection : MonoBehaviour
{
    [SerializeField] GameObject _skillLevelSelectionObject;

    private void Awake()
    {
        _skillLevelSelectionObject.SetActive(false);
    }

    public void Open()
    {
        _skillLevelSelectionObject.SetActive(true);
        PauseManager.SetPaused(true);
    }

    private void Close()
    {
        _skillLevelSelectionObject.SetActive(false);
        PauseManager.SetPaused(false);
    }

    public void LevelUpSkill(string skill)
    {
        if (skill == "attack") PlayerManager.Instance.Leveling.LevelUpSkill(1, 0, 0);
        else if (skill == "speed") PlayerManager.Instance.Leveling.LevelUpSkill(0, 1, 0);
        else if (skill == "luck") PlayerManager.Instance.Leveling.LevelUpSkill(0, 0, 1);
        Close();
    }
}
