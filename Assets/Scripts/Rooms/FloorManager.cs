using TMPro;
using UnityEngine;
using static FloorType;

public enum FloorType {Normal, Shop, Boss};

public class FloorManager : Singleton<FloorManager>
{
    [SerializeField] private FloorType[] _floorsTypes;
    private int _currentFloorActualIndex = 0;
    public static int CurrentFloor = 0;

    [SerializeField] private TextMeshProUGUI _floorNumberText;

    private void OnEnable()
    {
        WarpPortal.OnWarpToNextFloor += GenerateNextFloor;
    }

    private void OnDisable()
    {
        WarpPortal.OnWarpToNextFloor -= GenerateNextFloor;
    }

    private void Start()
    {
        GenerateNextFloor();
    }

    private void GenerateNextFloor()
    {
        if (_currentFloorActualIndex >= _floorsTypes.Length) return;

        FloorType type = _floorsTypes[_currentFloorActualIndex];
        if (type == Normal)
        {
            CurrentFloor++;
            RoomsDeckManager.Instance.GenerateFloorFromDeck();
        }

        _floorNumberText.text = CurrentFloor + "/F";

        _currentFloorActualIndex++;
    }
}