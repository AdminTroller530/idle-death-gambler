using TMPro;
using UnityEngine;

public class ChipsManager : Singleton<ChipsManager>
{
    private int _chipsAmount;
    private float _chipsDisplayAmount = 0;
    private const float CHIPS_DISPLAY_UPDATE_SPEED = 7f;
    [SerializeField] private TextMeshProUGUI _chipsText;

    private const float CHIPS_DROP_OFFSET_MAX = 1f;
    [SerializeField] private ChipPrefab _chipPrefab;

    protected override void Awake()
    {
        base.Awake();
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

    public void SpawnChip(Vector2 pos, int value)
    {
        Vector2 spawnPos = pos + new Vector2(Random.Range(-CHIPS_DROP_OFFSET_MAX, CHIPS_DROP_OFFSET_MAX), Random.Range(-CHIPS_DROP_OFFSET_MAX, CHIPS_DROP_OFFSET_MAX));
        ChipPrefab chip = Instantiate(_chipPrefab, spawnPos, transform.rotation);
        chip.Initialize(value);
    }
}
