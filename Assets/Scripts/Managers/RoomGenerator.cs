using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using static Direction;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private Transform _tileGrid;

    private GameObject[][] _allRooms;
    // room group named by where their exits are
    [SerializeField] private GameObject[] _hallsUpDown, _hallsLeftRight;
    [SerializeField] private GameObject[] _roomsUpDown, _roomsUpLeft, _roomsUpRight, _roomsDownLeft, _roomsDownRight, _roomsLeftRight;
    private Direction[] _roomExitDirs = {None, Up, Right, Down, Right, Right}; // for each _room above
    private Direction _previousExitDir = Right; // exit direction of previous room (start room by default)

    private Dictionary<Direction, int[]> _possibleNextRoomType = new Dictionary<Direction, int[]>()
    {
        // {_previousExitDir --> possible next _room choices}
        {Up, new int[]{0, 4}},
        {Down, new int[]{0, 2}},
        {Right, new int[]{1, 3, 5}}
    };

    public static int RoomsSpawned = 0;
    private Vector2Int _roomDimensions = new Vector2Int(20, 20);
    private int _hallLength = 10; // in unity units
    private Vector2Int _currentRoomPos = new Vector2Int(0, 0);

    [SerializeField] private LayerMask _wallMask;

    private void Awake()
    {
        _allRooms = new GameObject[][]{_roomsUpDown, _roomsUpLeft, _roomsUpRight, _roomsDownLeft, _roomsDownRight, _roomsLeftRight};

        // spawning a few rooms for debug
        for (int i = 0; i < 12; i++)
        {
            SpawnNextRoom();
        }
    }

    public void GenerateLevelFromRoomsDeck(List<RoomCardData> deck)
    {
        // TODO
    }

    private void AddRoomGraph(Vector2 center, int width, int depth)
    {
        GridGraph graph = AstarPath.active.data.AddGraph(typeof(GridGraph)) as GridGraph;

        graph.is2D = true;
        graph.collision.use2D = true;
        graph.collision.diameter = 3.5f;
        graph.collision.mask = _wallMask;
        graph.center = center;
        graph.SetDimensions(width * 2, depth * 2, 0.5f);
    }

    public void SpawnNextRoom()
    {
        // spawn hallway + update currentRoomPos
        if (_previousExitDir == Up) {
            Instantiate(_hallsUpDown[0], _currentRoomPos + (_roomDimensions.y + _hallLength)*0.5f * Vector2.up, transform.rotation, _tileGrid);
            _currentRoomPos += Vector2Int.up * (_roomDimensions.y + _hallLength);
        }
        else if (_previousExitDir == Down) {
            Instantiate(_hallsUpDown[0], _currentRoomPos - (_roomDimensions.y + _hallLength)*0.5f * Vector2.up, transform.rotation, _tileGrid);
            _currentRoomPos -= Vector2Int.up * (_roomDimensions.y + _hallLength);
        }
        else { // (_previousExitDir == Right)
            Instantiate(_hallsLeftRight[0], _currentRoomPos + (_roomDimensions.x + _hallLength)*0.5f * Vector2.right, transform.rotation, _tileGrid);
            _currentRoomPos += Vector2Int.right * (_roomDimensions.x + _hallLength);
        }
        
        // spawn room of a randome type from all possible continuations
        int type = _possibleNextRoomType[_previousExitDir][RNGController.GetMapRNG(0, _possibleNextRoomType[_previousExitDir].Length)];
        GameObject room = _allRooms[type][RNGController.GetMapRNG(0, _allRooms[type].Length)];
        Instantiate(room, (Vector2)_currentRoomPos, transform.rotation, _tileGrid);

        // add new A* pathfinding graph at new room
        AddRoomGraph(_currentRoomPos, _roomDimensions.x, _roomDimensions.y);

        // set new previousExit for next iteration
        if (type == 0) _previousExitDir = _previousExitDir == Up ? Up : Down; // for _roomsUpDown
        else _previousExitDir = _roomExitDirs[type];

        RoomsSpawned++;

        // Debug.Log("Type: " + type);
        // Debug.Log("Exit: " + previousExit);
    }
}
