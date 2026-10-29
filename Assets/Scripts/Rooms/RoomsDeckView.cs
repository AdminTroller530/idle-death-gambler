using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoomsDeckView : MonoBehaviour
{
    // 14 cards display max. dimensions: 7 horizontal, 2 vertical
    private const int HORIZONTAL_SPACING = 80;
    private const int FIRST_ROW_Y = 40;
    private const int SECOND_ROW_Y = -60;
    private int currentX = HORIZONTAL_SPACING * -3;

    [SerializeField] GameObject _roomsDeckViewObject;
    [SerializeField] private RoomCardDisplayPrefab _roomCardDisplayPrefab;

    private List<RoomCardDisplayPrefab> _roomCardPrefabs = new List<RoomCardDisplayPrefab>();

    private void OnEnable() {
        RoomsDeckManager.OnAddCardToDeck += AddCard;
        WarpPortal.OnWarpToNextFloor += ResetCardStates;
        RoomEnterTrigger.OnEnterNewRoom += DiscardNextCard;
    }
    private void OnDisable() {
        RoomsDeckManager.OnAddCardToDeck -= AddCard;
        WarpPortal.OnWarpToNextFloor -= ResetCardStates;
        RoomEnterTrigger.OnEnterNewRoom -= DiscardNextCard;
    }

    public void ButtonValuePressed(InputAction.CallbackContext context)
    {
        if (context.started) Open();
        if (context.canceled) Close();
    }

    private void Open()
    {
        _roomsDeckViewObject.SetActive(true);
    }

    private void Close()
    {
        _roomsDeckViewObject.SetActive(false);
    }

    private void DiscardNextCard()
    {
        _roomCardPrefabs[RoomsDeckManager.Instance.GetCurrentIndexShuffled()].SetDiscardedState(true);
    }

    private void ResetCardStates()
    {
        foreach (RoomCardDisplayPrefab prefab in _roomCardPrefabs) {prefab.SetDiscardedState(false);}
    }

    private void AddCard(RoomCardData card)
    {
        int currentY = RoomsDeckManager.Instance.GetDeckSize() <= 7 ? FIRST_ROW_Y : SECOND_ROW_Y;
        Vector2 pos = new Vector2(currentX, currentY);

        RoomCardDisplayPrefab cardPrefab = Instantiate(_roomCardDisplayPrefab, _roomsDeckViewObject.transform);
        _roomCardPrefabs.Add(cardPrefab);

        cardPrefab.transform.localPosition = pos;
        cardPrefab.SetSprite(card);
        cardPrefab.SetDiscardedState(false);

        if (currentX == HORIZONTAL_SPACING*3) currentX = -currentX;
        else currentX += HORIZONTAL_SPACING;
    }

    private void Awake()
    {
        _roomsDeckViewObject.SetActive(false);
    }
}
