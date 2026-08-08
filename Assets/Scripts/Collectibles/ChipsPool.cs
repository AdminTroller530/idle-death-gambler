using UnityEngine;
using UnityEngine.Pool;

public class ChipsPool : Singleton<ChipsPool>
{
    private const int CHIP_POOL_SIZE = 100;
    public ObjectPool<ChipPrefab> ChipPool;

    [SerializeField] private ChipPrefab _chipPrefab;

    protected override void Awake()
    {
        base.Awake();
    }

    private void InitializeBulletPool()
    {
        ChipPrefab[] initializedChips = new ChipPrefab[CHIP_POOL_SIZE];
        for (int i = 0; i < CHIP_POOL_SIZE; i++) initializedChips[i] = ChipPool.Get();
        for (int i = 0; i < CHIP_POOL_SIZE; i++) ChipPool.Release(initializedChips[i]);
    }

    private void Start()
    {
        ChipPool = new ObjectPool<ChipPrefab>(CreateChip, OnGetChip, OnReleaseChip, OnDestroyChip, true, CHIP_POOL_SIZE, CHIP_POOL_SIZE*2);
        InitializeBulletPool();
    }

    private ChipPrefab CreateChip()
    {
        ChipPrefab chip = Instantiate(_chipPrefab, Vector2.zero, Quaternion.identity, transform);
        return chip;
    }

    private void OnGetChip(ChipPrefab chip)
    {
        chip.gameObject.SetActive(true);
    }

    private void OnReleaseChip(ChipPrefab chip)
    {
        chip.gameObject.SetActive(false);
    }

    private void OnDestroyChip(ChipPrefab chip)
    {
        Destroy(chip);
    }
}
