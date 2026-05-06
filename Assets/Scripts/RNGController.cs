using System;
using UnityEngine;
using Random = System.Random;

public class RNGController : MonoBehaviour
{
    string seedString;
    bool useRandomSeed = true;
    Random mapRNG, itemRNG;
    int mapRNGCount, itemRNGcount;

    void Start()
    {
        int seed = useRandomSeed ? DateTime.Now.ToString().GetHashCode() : seedString.GetHashCode();
        
        mapRNG = new Random(seed);
        itemRNG = new Random(seed + 1);

        // Debug.Log(mapRNG.Next(1,100));
        // Debug.Log(mapRNG.Next(1,100));
        // Debug.Log(mapRNG.Next(1,100));
        
    }

    void GenerateRoom() // temp
    {
        int room = mapRNG.Next(1,100);
        mapRNGCount++;
    }

    void LoadState()
    {
        for (int i=0; i<mapRNGCount; i++)
        {
            mapRNG.Next(1,100);
        }
    }

}
