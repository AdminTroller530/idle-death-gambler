using System;
using UnityEngine;
using Random = System.Random;

public class RNGController : MonoBehaviour
{
    private static string _seedString;
    private static bool _useRandomSeed = true;
    private static Random _mapRNG;
    private static Random _itemRNG;
    private static int _mapRNGCount;
    private static int _itemRNGcount;

    private void Awake()
    {
        int seed = _useRandomSeed ? DateTime.Now.ToString().GetHashCode() : _seedString.GetHashCode();
        
        _mapRNG = new Random(seed);
        _itemRNG = new Random(seed + 1);
    }

    public static int GetMapRNG(int min, int max) // min inclusive, max exclusive
    {
        _mapRNGCount++;
        return _mapRNG.Next(min, max);
    }

    private void LoadState() // implement with file i/o in the future
    {
        for (int i=0; i<_mapRNGCount; i++)
        {
            _mapRNG.Next();
        }
    }

}
