using TMPro;
using UnityEngine;
using static FloorType;

public enum FloorType {Normal, Shop, Boss};

public class FloorManager : Singleton<FloorManager>
{
    [SerializeField] private FloorType[] _floorsTypes;
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
        if (CurrentFloor >= _floorsTypes.Length) return;

        FloorType type = _floorsTypes[CurrentFloor];
        if (type == Normal)
        {
            RoomsDeckManager.Instance.GenerateFloorFromDeck();
        }

        _floorNumberText.text = (CurrentFloor + 1) + "/F";

        CurrentFloor++;
    }
}