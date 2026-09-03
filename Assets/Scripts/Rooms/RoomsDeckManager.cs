using System.Collections.Generic;
using UnityEngine;
using static Direction;

public class RoomsDeckManager : MonoBehaviour
{
    [SerializeField] private List<RoomCardData> _allRoomCards;
    private List<RoomCardData> _roomsDeck = new List<RoomCardData>();
    private List<RoomCardData> _roomsDeckShuffled = new List<RoomCardData>();
    private int _roomsDeckCurrentIndex = 0;

    [SerializeField] private RoomGenerator _roomGenerator;

    private void InitializeRoomReferences()
    {
        for (int i = 0; i < _allRoomCards.Count; i++)
        {
            foreach (RoomData room in _allRoomCards[i].RoomPoolUp) {
                room.ExitTransform = room.RoomPrefab.transform.Find("Doors").Find("Exit");
                room.EnterTrigger = room.RoomPrefab.transform.Find("Enter Trigger").GetComponent<BoxCollider2D>();
            }
            foreach (RoomData room in _allRoomCards[i].RoomPoolDown) {
                room.ExitTransform = room.RoomPrefab.transform.Find("Doors").Find("Exit");
                room.EnterTrigger = room.RoomPrefab.transform.Find("Enter Trigger").GetComponent<BoxCollider2D>();
            }
            foreach (RoomData room in _allRoomCards[i].RoomPoolLeft) {
                room.ExitTransform = room.RoomPrefab.transform.Find("Doors").Find("Exit");
                room.EnterTrigger = room.RoomPrefab.transform.Find("Enter Trigger").GetComponent<BoxCollider2D>();
            }
        }
    }

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

    private void GenerateNextRoom()
    {
        if (_roomsDeckCurrentIndex >= _roomsDeckShuffled.Count) return;

        _roomGenerator.GenerateRoom(_roomsDeckShuffled[_roomsDeckCurrentIndex]);
        _roomsDeckCurrentIndex++;
    }

    private void AddCardToDeck(RoomCardData card)
    {
        _roomsDeck.Add(Instantiate(card));
    }

    private void Awake()
    {
        InitializeRoomReferences();
        AddCardToDeck(_allRoomCards[0]);
        AddCardToDeck(_allRoomCards[1]);

        _roomsDeckShuffled = _roomsDeck;
        // _roomsDeckShuffled = ShuffleRoomsDeck(_roomsDeck);
        GenerateNextRoom();
        GenerateNextRoom();
    }
}
