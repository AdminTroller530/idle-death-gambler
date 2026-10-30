using System;
using UnityEngine;
using Random = System.Random;

public class RNGController : MonoBehaviour
{
    private static string _seedString;
    private static bool _useRandomSeed = true;
    private static Random _mapRNG;
    private static Random _itemRNG;
    private static Random _roomCardRNG;
    private static Random _dealerRNG;
    private static int _mapRNGCount;
    private static int _itemRNGCount;
    private static int _roomCardRNGCount;
    private static int _dealerRNGCount;

    private void Awake()
    {
        int seed = _useRandomSeed ? DateTime.Now.ToString().GetHashCode() : _seedString.GetHashCode();
        
        _mapRNG = new Random(seed);
        _itemRNG = new Random(seed + 1);
        _roomCardRNG = new Random(seed + 2);
        _dealerRNG = new Random(seed + 3);
    }

    public static int GetMapRNG(int min, int max) // min inclusive, max exclusive
    {
        _mapRNGCount++;
        return _mapRNG.Next(min, max);
    }

    public static int GetRoomCardRNG(int min, int max) // min inclusive, max exclusive
    {
        _roomCardRNGCount++;
        return _roomCardRNG.Next(min, max);
    }

    public static int GetDealerRNG(int min, int max) // min inclusive, max exclusive
    {
        if (min >= max) return max - 1;
        
        _dealerRNGCount++;
        return _dealerRNG.Next(min, max);
    }

    private void LoadState() // implement with file i/o in the future
    {
        for (int i=0; i<_mapRNGCount; i++)
        {
            _mapRNG.Next();
            _roomCardRNG.Next();
            _dealerRNG.Next();
        }
    }

}
