using System;
using UnityEngine;
using Random = System.Random;

public class RNGController : MonoBehaviour
{
    static string seedString;
    static bool useRandomSeed = true;
    static Random mapRNG, itemRNG;
    static int mapRNGCount, itemRNGcount;

    void Awake()
    {
        int seed = useRandomSeed ? DateTime.Now.ToString().GetHashCode() : seedString.GetHashCode();
        
        mapRNG = new Random(seed);
        itemRNG = new Random(seed + 1);
    }

    public static int MapRNG(int min, int max) // min inclusive, max exclusive
    {
        mapRNGCount++;
        return mapRNG.Next(min, max);
    }

    void LoadState() // implemented with file i/o in the future
    {
        for (int i=0; i<mapRNGCount; i++)
        {
            mapRNG.Next();
        }
    }

}
