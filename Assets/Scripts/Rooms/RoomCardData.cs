using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomCardData", menuName = "Scriptable Objects/RoomCardData")]
public class RoomCardData : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Sprite;

    [Header("Room Pools (by entrance direction)")]
    public List<RoomData> RoomPoolUp;
    public List<RoomData> RoomPoolDown;
    public List<RoomData> RoomPoolLeft;
}
