using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Rarity;

public class DealerShopAddCard : MonoBehaviour
{
    private Dictionary<Rarity, int> _rarityWeights = new Dictionary<Rarity, int>{{Common, 30}, {Uncommon, 20}, {Rare, 7}, {Epic, 3}, {Legendary, 1}};

    [SerializeField] private List<RoomCardData> _allCards;
    private Dictionary<Rarity, List<RoomCardData>> _cardsPool = new Dictionary<Rarity, List<RoomCardData>>{};

    [SerializeField] private GameObject _uiObject;
    [SerializeField] private SpriteRenderer[] _cardOptionsSprites;

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
        _uiObject.SetActive(true);
        RoomCardData[] cardOptions = GenerateCardChoices(GenerateCardRarities());
        for (int i = 0; i < _cardOptionsSprites.Length; i++)
        {
            if (cardOptions[i] != null) _cardOptionsSprites[i].sprite = cardOptions[i].Sprite;
        }
    }

    private void Awake()
    {
        InitializeAllCardRarities();
    }
}
