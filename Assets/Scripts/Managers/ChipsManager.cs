using System;
using System.Linq;
using TMPro;
using UnityEngine;

public enum ChipValue
{
    White = 1,
    Red = 5,
    Blue = 10
}

public class ChipsManager : Singleton<ChipsManager>
{
    private int _chipsAmount;
    private float _chipsDisplayAmount = 0;
    private const float CHIPS_DISPLAY_UPDATE_SPEED = 7f;
    [SerializeField] private TextMeshProUGUI _chipsText;

    [SerializeField] private ChipPrefab _chipPrefab;

    private ChipValue[] _chipValues; // stores chip values in descending order

    protected override void Awake()
    {
        base.Awake();

        _chipValues = (ChipValue[]) Enum.GetValues(typeof(ChipValue));
        _chipValues = _chipValues.Reverse().ToArray();
        SpawnChips(Vector2.zero, 0);
    }

    private void Update()
    {
        _chipsDisplayAmount = Mathf.Lerp(_chipsDisplayAmount, _chipsAmount, CHIPS_DISPLAY_UPDATE_SPEED * Time.deltaTime);
        if (Mathf.Abs(_chipsAmount - _chipsDisplayAmount) < 0.4f) _chipsDisplayAmount = _chipsAmount;

        _chipsText.text = ((int)_chipsDisplayAmount).ToString();
    }

    public int GetChipsAmount() => _chipsAmount;

    public void SetChipsAmount(int amount) {_chipsAmount = amount;}

    public bool DecreaseChips(int amount)
    {
        if (_chipsAmount < amount) return false;
        _chipsAmount -= amount;
        return true;
    }

    public void IncreaseChips(int amount)
    {
        _chipsAmount += amount;
    }

    public void SpawnChips(Vector2 pos, int totalValue)
    {
        foreach (ChipValue chipValue in _chipValues)
        {
            while (totalValue >= (int)chipValue)
            {
                SpawnChip(pos, chipValue);
                totalValue -= (int)chipValue;
            }
        }
    }

    private void SpawnChip(Vector2 pos, ChipValue value)
    {
        ChipPrefab chip = ChipsPool.Instance.ChipPool.Get();
        chip.transform.position = pos;
        chip.Initialize(value);
    }
}
