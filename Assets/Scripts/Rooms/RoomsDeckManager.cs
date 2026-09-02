using System.Collections.Generic;
using UnityEngine;
using static Direction;

public class RoomsDeckManager : MonoBehaviour
{
    [SerializeField] private List<RoomCardData> _allRoomCards;
    private List<RoomCardData> _roomsDeck = new List<RoomCardData>();
    private List<RoomCardData> _roomsDeckShuffled = new List<RoomCardData>();

    [SerializeField] private RoomGenerator _roomGenerator;

    private List<RoomCardData> ShuffleRoomsDeck(List<RoomCardData> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int j = RNGController.GetRoomCardRNG(0, deck.Count);

            RoomCardData temp = deck[i];
            deck[i] = deck[j];
            deck[j] = temp;
        }
        
        return deck;
    }

    private void PrintDeck(List<RoomCardData> deck) // TEMP DEBUG
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Debug.Log(deck[i]);
        }
    }


    private void Awake()
    {
        _roomsDeck.Add(_allRoomCards[0]);
        _roomsDeck.Add(_allRoomCards[1]);
        _roomsDeck.Add(_allRoomCards[2]);

        _roomsDeckShuffled = ShuffleRoomsDeck(_roomsDeck);
        // PrintDeck(_roomsDeckShuffled);
        _roomGenerator.GenerateLevelFromRoomsDeck(_roomsDeckShuffled);
    }
}
