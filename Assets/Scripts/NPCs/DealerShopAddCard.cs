using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Rarity;

public class DealerShopAddCard : MonoBehaviour
{
    private Dictionary<Rarity, int> _rarityWeights = new Dictionary<Rarity, int>{{Common, 30}, {Uncommon, 20}, {Rare, 0}, {Epic, 0}, {Legendary, 0}};
    // private Dictionary<Rarity, int> _rarityWeights = new Dictionary<Rarity, int>{{Common, 30}, {Uncommon, 20}, {Rare, 7}, {Epic, 3}, {Legendary, 1}};

    [SerializeField] private List<RoomCardData> _allCards;
    private Dictionary<Rarity, List<RoomCardData>> _cardsPool = new Dictionary<Rarity, List<RoomCardData>>{};

    private RoomCardData[] _cardOptions = new RoomCardData[3];

    private bool _hasRolledCards;
    public bool HasChosenCard {private set; get;}

    [SerializeField] private GameObject _uiObject;
    [SerializeField] private GameObject _rollCardsButton;
    [SerializeField] private Image[] _cardOptionsSprites;
    [SerializeField] private Sprite _cardBackSprite;

    private void OnEnable() {WarpPortal.OnWarpToNextFloor += ResetCardRolls;}
    private void OnDisable() {WarpPortal.OnWarpToNextFloor -= ResetCardRolls;}

    private void ResetCardRolls() {
        for (int i = 0; i < _cardOptions.Length; i++)
        {
            _cardOptions[i] = null;
            _cardOptionsSprites[i].sprite = _cardBackSprite;
        }
        _hasRolledCards = false;
        HasChosenCard = false;
    }

    private Rarity[] GenerateCardRarities()
    {
        Rarity[] offeredRarities = new Rarity[3];

        for (int i = 0; i < offeredRarities.Length; i++)
        {

            int roll = RNGController.GetDealerRNG(1, _rarityWeights.Values.Sum() + 1);
            int sum = 0;

            foreach (Rarity rarity in _rarityWeights.Keys)
            {
                sum += _rarityWeights[rarity];
                if (roll <= sum)
                {
                    offeredRarities[i] = rarity;
                    break;
                }
            }
        }

        // foreach (Rarity rarity in offeredRarities) Debug.Log(rarity);

        return offeredRarities;
    }

    private RoomCardData[] GenerateCardChoices(Rarity[] generatedRarities)
    {
        RoomCardData[] cardOptions = new RoomCardData[3];
        for (int i = 0; i < generatedRarities.Length; i++)
        {
            Rarity rarity = generatedRarities[i];
            
            if (!_cardsPool.ContainsKey(rarity)) continue;
            cardOptions[i] = _cardsPool[rarity][RNGController.GetDealerRNG(0, _cardsPool[rarity].Count)];
        }
        return cardOptions;
    }

    private void InitializeAllCardRarities()
    {
        for (int i = 0; i < _allCards.Count; i++)
        {
            RoomCardData card = _allCards[i];
            if (_cardsPool.ContainsKey(card.Rarity)) _cardsPool[card.Rarity].Add(card);
            else _cardsPool[card.Rarity] = new List<RoomCardData>(){card};
        }
    }

    public void RollShopCards()
    {
        _cardOptions = GenerateCardChoices(GenerateCardRarities());
        for (int i = 0; i < _cardOptionsSprites.Length; i++)
        {
            if (_cardOptions[i] != null) _cardOptionsSprites[i].sprite = _cardOptions[i].Sprite;
        }

        _hasRolledCards = true;
        _rollCardsButton.SetActive(false);
    }

    public void SelectCard(int index)
    {
        if (!_hasRolledCards) return;
        HasChosenCard = true;
        RoomsDeckManager.Instance.AddCardToDeck(_cardOptions[index]);
        DealerShopManager.Instance.Close();
    }

    public void OpenMenu()
    {
        _uiObject.SetActive(true);
        _rollCardsButton.SetActive(!_hasRolledCards);
    }

    private void Awake()
    {
        InitializeAllCardRarities();
        ResetCardRolls();
    }
}
