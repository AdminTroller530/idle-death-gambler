using Pathfinding;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private Transform _tileGrid;

    // room group named by where their exits are
    [SerializeField] private GameObject[] _hallsUpDown, _hallsLeftRight;
    [SerializeField] private GameObject[] _roomsUpDown, _roomsUpLeft, _roomsUpRight, _roomsDownLeft, _roomsDownRight, _roomsLeftRight;
    private int[] _roomExitDirs = {-1, 0, 2, 1, 2, 2}; // up: 0, down: 1, right: 2
    private GameObject[][] _allRooms;

    public static int RoomsSpawned = 0;

    private int _previousExit = 2; // up: 0, down: 1, right: 2
    private int[][] _possibleNextRoomType = { // up, down, right - previous exit can lead to what rooms?
        new int[]{0, 4},
        new int[]{0, 2},
        new int[]{1, 3, 5}
    };

    private Vector2Int _roomDimensions = new Vector2Int(20, 20);
    private int _hallLength = 10; // in unity units
    private Vector2Int _currentRoomPos = new Vector2Int(0, 0);

    [SerializeField] private LayerMask _wallMask;

    private void Awake()
    {
        _allRooms = new GameObject[][]{_roomsUpDown, _roomsUpLeft, _roomsUpRight, _roomsDownLeft, _roomsDownRight, _roomsLeftRight};

        // test create new gridgraph in pathfinder graphs
        // AddRoomGraph(new Vector2Int(5, 0), 50, 50);

        // spawning a few rooms for debug
        for (int i = 0; i < 12; i++)
        {
            SpawnNextRoom();
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

    public void SpawnNextRoom()
    {

        // spawn hallway + update currentRoomPos
        if (_previousExit == 0) { // up
            Instantiate(_hallsUpDown[0], _currentRoomPos + (_roomDimensions.y + _hallLength)*0.5f * Vector2.up, transform.rotation, _tileGrid);
            _currentRoomPos += Vector2Int.up * (_roomDimensions.y + _hallLength);
        }
        else if (_previousExit == 1) { // down
            Instantiate(_hallsUpDown[0], _currentRoomPos - (_roomDimensions.y + _hallLength)*0.5f * Vector2.up, transform.rotation, _tileGrid);
            _currentRoomPos -= Vector2Int.up * (_roomDimensions.y + _hallLength);
        }
        else { // right
            Instantiate(_hallsLeftRight[0], _currentRoomPos + (_roomDimensions.x + _hallLength)*0.5f * Vector2.right, transform.rotation, _tileGrid);
            _currentRoomPos += Vector2Int.right * (_roomDimensions.x + _hallLength);
        }
        
        // spawn room of a randome type from all possible continuations
        int type = _possibleNextRoomType[_previousExit][RNGController.GetMapRNG(0, _possibleNextRoomType[_previousExit].Length)];
        GameObject room = _allRooms[type][RNGController.GetMapRNG(0, _allRooms[type].Length)];
        Instantiate(room, (Vector2)_currentRoomPos, transform.rotation, _tileGrid);

        // add new A* pathfinding graph at new room
        AddRoomGraph(_currentRoomPos, _roomDimensions.x, _roomDimensions.y);

        // set new previousExit for next iteration
        if (type == 0) _previousExit = _previousExit == 0 ? 0 : 1;
        else _previousExit = _roomExitDirs[type];

        RoomsSpawned++;

        // Debug.Log("Type: " + type);
        // Debug.Log("Exit: " + previousExit);
    }
}
