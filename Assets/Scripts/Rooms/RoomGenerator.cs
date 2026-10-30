using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using static Direction;

public class RoomGenerator : Singleton<RoomGenerator>
{
    [SerializeField] private Transform _tileGrid;

    [SerializeField] private GameObject[] _hallsUpDown, _hallsLeftRight;
    [SerializeField] private List<GameObject> _shops;
    private Direction _previousExitDir = Right; // exit direction of previous room (start room by default)

    [SerializeField] private GameObject _entranceRoom;
    [SerializeField] private GameObject _warpRoom;

    public static int RoomsSpawned = 0;
    private int _hallLength = 10; // in unity units
    private Vector2 _currentRoomPos = new Vector2(0, 0);
    private const int ENTRANCE_ROOM_LENGTH = 20;

    [SerializeField] private LayerMask _wallMask;

    private List<GameObject> _activeRooms = new List<GameObject>();

    private void OnEnable()
    {
        WarpPortal.OnWarpToNextFloor += ResetCurrentRoomPos;        
    }

    private void OnDisable()
    {
        WarpPortal.OnWarpToNextFloor -= ResetCurrentRoomPos;        
    }

    private void ResetCurrentRoomPos() {_currentRoomPos = Vector2.zero;}

    public void GenerateEntranceRoom()
    {
        _activeRooms.Add(Instantiate(_entranceRoom, _currentRoomPos, Quaternion.identity, _tileGrid));
        
        AddAStarGraph(_currentRoomPos, ENTRANCE_ROOM_LENGTH, ENTRANCE_ROOM_LENGTH);
        _currentRoomPos += Vector2.right * ENTRANCE_ROOM_LENGTH/2;

        Direction _exitDirection = Right; // change based on chosen entrance room
        GenerateNextHallway(_exitDirection);
        _previousExitDir = _exitDirection;
    }

    public void GenerateShop()
    {
        GameObject shop = _shops[RNGController.GetMapRNG(0, _shops.Count)];
        _activeRooms.Add(Instantiate(shop, Vector2.zero, Quaternion.identity, _tileGrid));
    }

    public bool GenerateRoomFromCard(RoomCardData roomCard)
    {
        List<RoomData> currentRoomPool;
        Direction entranceDir = _previousExitDir.Flip();

        // choose correct room pool
        if (entranceDir == Up) currentRoomPool = roomCard.RoomPoolUp;
        else if (entranceDir == Down) currentRoomPool = roomCard.RoomPoolDown;
        else if (entranceDir == Left) currentRoomPool = roomCard.RoomPoolLeft;
        else return false; // fallback

        // if there are no rooms to choose from 
        if (currentRoomPool.Count == 0) return false;

        // randomly select room from pool and instantiate it
        RoomData room = currentRoomPool[RNGController.GetMapRNG(0, currentRoomPool.Count)];
        _activeRooms.Add(Instantiate(room.RoomPrefab, _currentRoomPos, Quaternion.identity, _tileGrid));

        AddAStarGraph(_currentRoomPos + room.EnterTrigger.offset, (int)room.EnterTrigger.size.x, (int)room.EnterTrigger.size.y);

        // update current room pos for next room
        _currentRoomPos += (Vector2)room.ExitTransform.localPosition + room.ExitDirection.ToDirectionVector()*0.5f;

        GenerateNextHallway(room.ExitDirection);

        _previousExitDir = room.ExitDirection;
        RoomsSpawned++;
        return true;
    }

    public void GenerateWarpRoom()
    {
        _currentRoomPos += _previousExitDir.ToDirectionVector() * _hallLength;
        _activeRooms.Add(Instantiate(_warpRoom, _currentRoomPos, Quaternion.identity, _tileGrid));
    }

    private void GenerateNextHallway(Direction exitDirection)
    {
        if (exitDirection == Up) {
            _activeRooms.Add(Instantiate(_hallsUpDown[0], _currentRoomPos + (_hallLength*0.5f * Vector2.up), transform.rotation, _tileGrid));
            _currentRoomPos += Vector2.up * _hallLength;
        }
        else if (exitDirection == Down) {
            _activeRooms.Add(Instantiate(_hallsUpDown[0], _currentRoomPos - (_hallLength*0.5f * Vector2.up), transform.rotation, _tileGrid));
            _currentRoomPos -= Vector2.up * _hallLength;
        }
        else { // (exitDirection == Right)
            _activeRooms.Add(Instantiate(_hallsLeftRight[0], _currentRoomPos + _hallLength*0.5f * Vector2.right, transform.rotation, _tileGrid));
            _currentRoomPos += Vector2.right * _hallLength;
        }
    }

    private void AddAStarGraph(Vector2 center, int width, int depth)
    {
        GridGraph graph = AstarPath.active.data.AddGraph(typeof(GridGraph)) as GridGraph;

        graph.is2D = true;
        graph.collision.use2D = true;
        graph.collision.diameter = 3.5f;
        graph.collision.mask = _wallMask;
        graph.center = center;
        graph.SetDimensions(width * 2, depth * 2, 0.5f);

        AstarPath.active.Scan(graph);
    }

    public void DeleteActiveRooms()
    {
        foreach (GameObject room in _activeRooms) {Destroy(room);}
        _activeRooms.Clear();
        
        foreach (NavGraph graph in AstarPath.active.data.graphs) {if (graph != null) AstarPath.active.data.RemoveGraph(graph);}
    }
}