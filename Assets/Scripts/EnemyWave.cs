using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawn
{
    public int id;
    public Vector2 pos;
}

[System.Serializable]
public class EnemyWave
{
    public List<EnemySpawn> spawns;
}
