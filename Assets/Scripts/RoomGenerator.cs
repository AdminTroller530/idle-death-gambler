using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] Transform tileGrid;

    // room group named by where their exits are
    [SerializeField] GameObject[] UD, UL, UR, DL, DR, LR;
    GameObject[][] rooms;
    [SerializeField] GameObject[] hallsUD, hallsLR;

    int[][] exitMapping = { // top to bottom: up, down, right - exit can go to what room (index)?
        new int[]{0, 4},
        new int[]{0, 2},
        new int[]{1, 3, 5}
    };
    int previousExit = 2; // up, down, right

    Vector2 roomDims = new Vector2(20, 20); // dimensions of rooms (width, height)
    int hallLength = 10;
    Vector2 roomPos; // position of next room to spawn

    void Start()
    {
        roomPos = new Vector2(roomDims.x + hallLength, 0);
        rooms = new GameObject[][]{UD, UL, UR, DL, DR, LR};

        SpawnRoom();
    }

    void SpawnRoom()
    {
        if (previousExit == 0) Instantiate(hallsUD[0], roomPos - (roomDims.y + hallLength)*0.5f * Vector2.up, transform.rotation, tileGrid);
        else if (previousExit == 1) Instantiate(hallsUD[0], roomPos + (roomDims.y + hallLength)*0.5f * Vector2.up, transform.rotation, tileGrid);
        else Instantiate(hallsLR[0], roomPos + (roomDims.x + hallLength)*0.5f * Vector2.left, transform.rotation, tileGrid);

        int type = exitMapping[previousExit][RNGController.MapRNG(0, exitMapping[previousExit].Length)];
        // Debug.Log("type: " + type);
        GameObject room = rooms[type][RNGController.MapRNG(0, rooms[type].Length)];
        Instantiate(room, roomPos, transform.rotation, tileGrid);


    }
}
