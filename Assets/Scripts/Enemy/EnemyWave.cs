using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawn
{
    public int Id;
    public Vector2 Pos;
}

[System.Serializable]
public class EnemyWave
{
    public List<EnemySpawn> Spawns;
}
