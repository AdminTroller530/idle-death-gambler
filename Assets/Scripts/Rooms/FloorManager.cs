using System;
using TMPro;
using UnityEngine;
using static FloorType;

public enum FloorType {Normal, Shop, Boss};

public class FloorManager : Singleton<FloorManager>
{
    [SerializeField] private FloorType[] _floorsTypes;
    private int _currentFloorActualIndex = 0;
    public int CurrentFloor {private set; get;} = 0;
    public FloorType CurrentFloorType {private set; get;}

    [SerializeField] private TextMeshProUGUI _floorNumberText;

    [NonSerialized] public FloorTimer Timer;

    private void OnEnable()
    {
        WarpPortal.OnWarpToNextFloor += GenerateNextFloor;
    }

    private void OnDisable()
    {
        WarpPortal.OnWarpToNextFloor -= GenerateNextFloor;
    }

    protected override void Awake()
    {
        base.Awake();
        Timer = GetComponent<FloorTimer>();
    }

    private void Start()
    {
        GenerateNextFloor();
    }

    private void GenerateNextFloor()
    {
        if (_currentFloorActualIndex >= _floorsTypes.Length) return;

        FloorType type = _floorsTypes[_currentFloorActualIndex];
        CurrentFloorType = type;
        
        if (type == Normal)
        {
            CurrentFloor++;
            Timer.StartFloorTimer();
            RoomsDeckManager.Instance.GenerateFloorFromDeck();
        }
        else if (type == Shop)
        {
            RoomGenerator.Instance.GenerateShop();
            Timer.StopTimer();
        }

        _floorNumberText.text = CurrentFloor + "/F";

        _currentFloorActualIndex++;
    }
}