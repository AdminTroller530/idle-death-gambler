using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomCardData", menuName = "Scriptable Objects/RoomCardData")]
public class RoomCardData : ScriptableObject
{
    public string Name;
    public string Description;
    public List<RoomData> RoomPool;
    public Sprite Sprite;
}
