using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "Scriptable Objects/RoomData")]
public class RoomData : ScriptableObject
{
    public string Name;
    public GameObject RoomPrefab;
    public Direction EntranceDirection;
    public Direction ExitDirection;
    public Transform ExitTransform;
}
