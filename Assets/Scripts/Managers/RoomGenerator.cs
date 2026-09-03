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
    private Vector2 _roomDimensions = new Vector2(20, 20);
    private int _hallLength = 10; // in unity units
    private Vector2 _currentRoomPos = new Vector2(10, 0);

    private Vector2 WEIRD_UPDOWN_HALLWAY_OFFSET = new Vector2(0.5f, 0.5f);

    [SerializeField] private LayerMask _wallMask;

    private void Awake()
    {
        _allRooms = new GameObject[][]{_roomsUpDown, _roomsUpLeft, _roomsUpRight, _roomsDownLeft, _roomsDownRight, _roomsLeftRight};

        // spawning a few rooms for debug
        for (int i = 0; i < 12; i++)
        {
            // SpawnNextRoom();
        }
    }

    public void GenerateRoom(RoomCardData roomCard)
    {
        List<RoomData> currentRoomPool;
        Direction entranceDir = _previousExitDir.Flip();

        // choose correct room pool
        if (entranceDir == Up) currentRoomPool = roomCard.RoomPoolUp;
        else if (entranceDir == Down) currentRoomPool = roomCard.RoomPoolDown;
        else if (entranceDir == Left) currentRoomPool = roomCard.RoomPoolLeft;
        else return; // fallback

        // randomly select room from pool and instantiate it
        RoomData room = currentRoomPool[RNGController.GetMapRNG(0, currentRoomPool.Count)];
        Instantiate(room.RoomPrefab, _currentRoomPos, Quaternion.identity, _tileGrid);

        // add room to A* pathfinding grid
        AddRoomGraph(_currentRoomPos + room.EnterTrigger.offset, (int)room.EnterTrigger.size.x, (int)room.EnterTrigger.size.y);
        
        // update current room pos for next room
        _currentRoomPos += (Vector2)room.ExitTransform.localPosition;
        _currentRoomPos += room.ExitDirection.ToDirectionVector();

        GenerateNextHallway(room.ExitDirection);

        _previousExitDir = room.ExitDirection;
        RoomsSpawned++;
    }

    private void GenerateNextHallway(Direction exitDirection)
    {
        if (exitDirection == Up) {
            Instantiate(_hallsUpDown[0], _currentRoomPos + _hallLength*0.5f * Vector2.up + WEIRD_UPDOWN_HALLWAY_OFFSET, transform.rotation, _tileGrid);
            _currentRoomPos += Vector2.up * _hallLength;
        }
        else if (exitDirection == Down) {
            Instantiate(_hallsUpDown[0], _currentRoomPos - _hallLength*0.5f * Vector2.up + WEIRD_UPDOWN_HALLWAY_OFFSET, transform.rotation, _tileGrid);
            _currentRoomPos -= Vector2.up * _hallLength;
        }
        else { // (exitDirection == Right)
            Instantiate(_hallsLeftRight[0], _currentRoomPos + _hallLength*0.5f * Vector2.right, transform.rotation, _tileGrid);
            _currentRoomPos += Vector2.right * _hallLength;
        }
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

    // OBSOLETE
    public void SpawnNextRoom()
    {
        // spawn hallway + update currentRoomPos
        if (_previousExitDir == Up) {
            Instantiate(_hallsUpDown[0], _currentRoomPos + (_roomDimensions.y + _hallLength)*0.5f * Vector2.up, transform.rotation, _tileGrid);
            _currentRoomPos += Vector2.up * (_roomDimensions.y + _hallLength);
        }
        else if (_previousExitDir == Down) {
            Instantiate(_hallsUpDown[0], _currentRoomPos - (_roomDimensions.y + _hallLength)*0.5f * Vector2.up, transform.rotation, _tileGrid);
            _currentRoomPos -= Vector2.up * (_roomDimensions.y + _hallLength);
        }
        else { // (_previousExitDir == Right)
            Instantiate(_hallsLeftRight[0], _currentRoomPos + (_roomDimensions.x + _hallLength)*0.5f * Vector2.right, transform.rotation, _tileGrid);
            _currentRoomPos += Vector2.right * (_roomDimensions.x + _hallLength);
        }
        
        // spawn room of a randome type from all possible continuations
        int type = _possibleNextRoomType[_previousExitDir][RNGController.GetMapRNG(0, _possibleNextRoomType[_previousExitDir].Length)];
        GameObject room = _allRooms[type][RNGController.GetMapRNG(0, _allRooms[type].Length)];
        Instantiate(room, (Vector2)_currentRoomPos, transform.rotation, _tileGrid);

        // add new A* pathfinding graph at new room
        AddRoomGraph(_currentRoomPos, (int)_roomDimensions.x, (int)_roomDimensions.y);

        // set new previousExit for next iteration
        if (type == 0) _previousExitDir = _previousExitDir == Up ? Up : Down; // for _roomsUpDown
        else _previousExitDir = _roomExitDirs[type];

        RoomsSpawned++;

        // Debug.Log("Type: " + type);
        // Debug.Log("Exit: " + previousExit);
    }
}
