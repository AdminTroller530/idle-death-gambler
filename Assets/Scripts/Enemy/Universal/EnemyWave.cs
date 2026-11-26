using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawn
{
    public int Id;
    public Transform SpawnPointTransform;
}

[System.Serializable]
public class EnemyWave
{
    public List<EnemySpawn> Spawns;
}

[System.Serializable]
public class EnemyWaves
{
    public string RoomCardName;
    public List<EnemyWave> Waves;
}