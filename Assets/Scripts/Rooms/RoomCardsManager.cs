using System.Collections.Generic;
using UnityEngine;
using static Direction;

public class RoomCardsManager : MonoBehaviour
{
    [SerializeField] private List<RoomCardData> _roomCards;

    private void ShuffleRoomsDeck()
    {
        for (int i = 0; i < _roomCards.Count; i++)
        {
            int j = RNGController.GetRoomCardRNG(0, _roomCards.Count);

            RoomCardData temp = _roomCards[i];
            _roomCards[i] = _roomCards[j];
            _roomCards[j] = temp;
        }
        
        for (int i = 0; i < _roomCards.Count; i++)
        {
            Debug.Log(_roomCards[i]);
        }
    }

    private void Awake()
    {
        ShuffleRoomsDeck();
    }
}
