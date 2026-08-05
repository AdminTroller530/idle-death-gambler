using TMPro;
using UnityEngine;

public class ChipsManager : MonoBehaviour
{
    private static int _chipsAmount;
    private float _chipsDisplayAmount = 0;
    private const float CHIPS_DISPLAY_UPDATE_SPEED = 7f;
    [SerializeField] private TextMeshProUGUI _chipsText;

    private void Update()
    {
        _chipsDisplayAmount = Mathf.Lerp(_chipsDisplayAmount, _chipsAmount, CHIPS_DISPLAY_UPDATE_SPEED * Time.deltaTime);
        if (Mathf.Abs(_chipsAmount - _chipsDisplayAmount) < 0.4f) _chipsDisplayAmount = _chipsAmount;

        _chipsText.text = ((int)_chipsDisplayAmount).ToString();
    }

    public static int GetChipsAmount() => _chipsAmount;

    public static void SetChipsAmount(int amount) {_chipsAmount = amount;}

    public static bool DecreaseChips(int amount)
    {
        if (_chipsAmount < amount) return false;
        _chipsAmount -= amount;
        return true;
    }

    public static void IncreaseChips(int amount)
    {
        _chipsAmount += amount;
    }
}
