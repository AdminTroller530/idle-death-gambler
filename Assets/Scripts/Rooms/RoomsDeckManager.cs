using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Direction;

public class RoomsDeckManager : MonoBehaviour
{
    [SerializeField] private List<RoomCardData> _allRoomCards;
    private List<RoomCardData> _roomsDeck = new List<RoomCardData>();
    private List<RoomCardData> _roomsDeckShuffled = new List<RoomCardData>();
    private List<int> _roomsDeckShuffledIndexMapping = new List<int>(); // maps shuffled indexes back to original deck indexes
    private int _roomsDeckCurrentIndex = 0;

    [SerializeField] private RoomGenerator _roomGenerator;

    private RoomDeckAnimation _roomDeckAnimation;

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

    private void Awake()
    {
        _roomDeckAnimation = GetComponent<RoomDeckAnimation>();

        InitializeRoomReferences();
        AddCardToDeck(_allRoomCards[0]);
        AddCardToDeck(_allRoomCards[1]);

        _roomsDeckShuffled = _roomsDeck;
        _roomsDeckShuffled = ShuffleRoomsDeck(_roomsDeck);

        StartCoroutine(GenerateNextRoom());
        // StartCoroutine(GenerateNextRoom());
    }

    private List<RoomCardData> ShuffleRoomsDeck(List<RoomCardData> originalDeck)
    {
        List<RoomCardData> deck = new List<RoomCardData>(originalDeck);

        _roomsDeckShuffledIndexMapping.Clear();
        for (int i = 0; i < deck.Count; i++) _roomsDeckShuffledIndexMapping.Add(i);

        for (int i = 0; i < deck.Count; i++)
        {
            int j = RNGController.GetRoomCardRNG(0, deck.Count);

            RoomCardData tempCard = deck[i];
            deck[i] = deck[j];
            deck[j] = tempCard;

            int tempIndex = _roomsDeckShuffledIndexMapping[i];
            _roomsDeckShuffledIndexMapping[i] = _roomsDeckShuffledIndexMapping[j];
            _roomsDeckShuffledIndexMapping[j] = tempIndex;
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

    private IEnumerator GenerateNextRoom()
    {
        if (_roomsDeckCurrentIndex >= _roomsDeckShuffled.Count) yield break;

        RoomCardData card = _roomsDeckShuffled[_roomsDeckCurrentIndex];

        yield return StartCoroutine(_roomDeckAnimation.DeckEnterAnimation(card));

        _roomGenerator.GenerateRoomFromCard(card);
        _roomsDeckCurrentIndex++;

        yield return StartCoroutine(_roomDeckAnimation.DeckExitAnimation());

    }

    private void AddCardToDeck(RoomCardData card)
    {
        _roomsDeck.Add(Instantiate(card));
    }
}
