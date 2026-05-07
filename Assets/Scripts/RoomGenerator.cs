using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] Transform tileGrid;

    // room group named by where their exits are
    [SerializeField] GameObject[] UD, UL, UR, DL, DR, LR;
    int[] roomExits = {-1, 0, 2, 1, 2, 2}; // up: 0, down: 1, right: 2
    GameObject[][] rooms;
    [SerializeField] GameObject[] hallsUD, hallsLR;

    public static int roomsSpawned = 0;

    int previousExit = 2; // up: 0, down: 1, right: 2
    int[][] exitMapping = { // up, down, right - previous exit can lead to what rooms?
        new int[]{0, 4},
        new int[]{0, 2},
        new int[]{1, 3, 5}
    };

    Vector2 roomDims = new Vector2(20, 20); // dimensions of rooms (width, height)
    int hallLength = 10;
    Vector2 currentPos = new Vector2(0, 0); // position of current room

    void Start()
    {
        rooms = new GameObject[][]{UD, UL, UR, DL, DR, LR};

        // spawning a few rooms for debug
        SpawnNextRoom();
        SpawnNextRoom();
        SpawnNextRoom();
        SpawnNextRoom();
        SpawnNextRoom();
        SpawnNextRoom();
        SpawnNextRoom();
        SpawnNextRoom();
        SpawnNextRoom();
        SpawnNextRoom();
        SpawnNextRoom();
        SpawnNextRoom();
    }

    public void SpawnNextRoom()
    {

        // spawn hallway + update currentPos
        if (previousExit == 0) { // up
            Instantiate(hallsUD[0], currentPos + (roomDims.y + hallLength)*0.5f * Vector2.up, transform.rotation, tileGrid);
            currentPos += Vector2.up * (roomDims.y + hallLength);
        }
        else if (previousExit == 1) { // down
            Instantiate(hallsUD[0], currentPos - (roomDims.y + hallLength)*0.5f * Vector2.up, transform.rotation, tileGrid);
            currentPos -= Vector2.up * (roomDims.y + hallLength);
        }
        else { // right
            Instantiate(hallsLR[0], currentPos + (roomDims.x + hallLength)*0.5f * Vector2.right, transform.rotation, tileGrid);
            currentPos += Vector2.right * (roomDims.x + hallLength);
        }
        
        // select room type randomly from all possible continuations
        int type = exitMapping[previousExit][RNGController.MapRNG(0, exitMapping[previousExit].Length)];
        GameObject room = rooms[type][RNGController.MapRNG(0, rooms[type].Length)];
        Instantiate(room, currentPos, transform.rotation, tileGrid);

        // set new previousExit for next iteration
        if (type == 0) previousExit = previousExit == 0 ? 0 : 1;
        else previousExit = roomExits[type];

        roomsSpawned++;

        // Debug.Log("Type: " + type);
        // Debug.Log("Exit: " + previousExit);
    }
}
