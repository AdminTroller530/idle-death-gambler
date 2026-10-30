using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Direction;

public class RoomsDeckManager : Singleton<RoomsDeckManager>
{
    [SerializeField] private List<RoomCardData> _allRoomCards;
    private List<RoomCardData> _roomsDeck = new List<RoomCardData>();
    private List<RoomCardData> _roomsDeckShuffled = new List<RoomCardData>();
    private List<int> _roomsDeckShuffledIndexMapping = new List<int>(); // maps shuffled indexes back to original deck indexes
    private int _roomsDeckCurrentIndex = 0;

    private RoomDeckAnimation _roomDeckAnimation;
    private Coroutine _enterAnimationCoroutine;

    public static event Action<RoomCardData> OnAddCardToDeck;

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

    protected override void Awake()
    {
        base.Awake();

        _roomDeckAnimation = GetComponent<RoomDeckAnimation>();

        InitializeRoomReferences();
        AddCardToDeck(Instantiate(_allRoomCards[0]));
        AddCardToDeck(Instantiate(_allRoomCards[1]));

        // _roomsDeckShuffled = ShuffleRoomsDeck(_roomsDeck);
    }

    private void Start()
    {
        // GenerateFloorFromDeck();
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
        
        _roomsDeckCurrentIndex = 0;
        return deck;
    }

    private void PrintDeck(List<RoomCardData> deck) // TEMP DEBUG
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Debug.Log(deck[i]);
        }
    }

    public void GenerateFloorFromDeck()
    {
        _roomsDeckShuffled = ShuffleRoomsDeck(_roomsDeck);
        _roomsDeckCurrentIndex = 0;

        RoomGenerator.Instance.GenerateEntranceRoom();
        for (int i = 0; i < _roomsDeckShuffled.Count; i++)
        {
            RoomCardData card = _roomsDeckShuffled[i];
            if (!RoomGenerator.Instance.GenerateRoomFromCard(card)) break;
        }
        RoomGenerator.Instance.GenerateWarpRoom();
    }

    public int GetCurrentIndexShuffled()
    {
        return _roomsDeckShuffledIndexMapping[_roomsDeckCurrentIndex];
    }

    public void RoomDeckEnterAnimation()
    {
        if (_roomsDeckCurrentIndex >= _roomsDeckShuffled.Count) return;

        RoomCardData card = _roomsDeckShuffled[_roomsDeckCurrentIndex];

        _enterAnimationCoroutine = StartCoroutine(_roomDeckAnimation.DeckEnterAnimation(card));
    }

    public void RoomDeckExitAnimation()
    {
        _roomsDeckCurrentIndex++;
        StopCoroutine(_enterAnimationCoroutine);
        StartCoroutine(_roomDeckAnimation.DeckExitAnimation());
    }

    public void AddCardToDeck(RoomCardData card)
    {
        _roomsDeck.Add(Instantiate(card));
        OnAddCardToDeck?.Invoke(card);
    }

    public int GetDeckSize() {return _roomsDeck.Count;}
}
