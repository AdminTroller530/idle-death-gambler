using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Rarity;
using static RoomCardEnchantment;

public class DealerShopAddCard : MonoBehaviour
{
    private Dictionary<Rarity, int> _rarityWeights = new Dictionary<Rarity, int>{{Common, 30}, {Uncommon, 20}, {Rare, 0}, {Epic, 0}, {Legendary, 0}};
    // private Dictionary<Rarity, int> _rarityWeights = new Dictionary<Rarity, int>{{Common, 30}, {Uncommon, 20}, {Rare, 7}, {Epic, 3}, {Legendary, 1}};

    [SerializeField] private List<RoomCardData> _allCards;
    private Dictionary<Rarity, List<RoomCardData>> _cardsPool = new Dictionary<Rarity, List<RoomCardData>>{};

    [SerializeField] private GameObject _cardEnchantmentIconPrefab;
    private List<GameObject> _cardEnchantmentIcons = new List<GameObject>();
    private RoomCardEnchantment[] _possibleEnchantments = {Greedy};

    private RoomCardData[] _cardOptions = new RoomCardData[3];

    private bool _hasRolledCards;
    public bool HasChosenCard {private set; get;}

    private int _cost;
    private const int BASE_COST = 15;

    [SerializeField] private GameObject _uiObject;
    [SerializeField] private GameObject _rollCardsButton;
    [SerializeField] private TextMeshProUGUI _rollCardsCostText;
    [SerializeField] private Image[] _cardOptionsSprites;
    [SerializeField] private Sprite _cardBackSprite;

    private void OnEnable() {WarpPortal.OnWarpToNextFloor += ResetCardRolls;}
    private void OnDisable() {WarpPortal.OnWarpToNextFloor -= ResetCardRolls;}

    private void ResetCardRolls()
    {
        for (int i = 0; i < _cardOptions.Length; i++)
        {
            _cardOptions[i] = null;
            _cardOptionsSprites[i].sprite = _cardBackSprite;
        }

        for (int i = 0; i < _cardEnchantmentIcons.Count; i++) {Destroy(_cardEnchantmentIcons[i]);}
        _cardEnchantmentIcons.Clear();

        _hasRolledCards = false;
        HasChosenCard = false;
    }

    private Rarity[] GenerateCardRarities()
    {
        Rarity[] offeredRarities = new Rarity[3];
        int weightsSum = _rarityWeights.Values.Sum();
        // luckBoost is a percentage of total sum of rarityweights; percentage scaled with luck level
        int luckBoost = Mathf.CeilToInt(weightsSum * (PlayerManager.Instance.Leveling.CalculateLuckPercent(PlayerManager.Instance.Stats.LuckLevel) / 100f) );

        for (int i = 0; i < offeredRarities.Length; i++)
        {
            int roll = RNGController.GetDealerRNG(luckBoost, weightsSum + 1);
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

            RoomCardData card = Instantiate(_cardsPool[rarity][RNGController.GetDealerRNG(0, _cardsPool[rarity].Count)]);
            RollCardForEnchantments(card);
            
            cardOptions[i] = card;
        }
        return cardOptions;
    }

    private void RollCardForEnchantments(RoomCardData card)
    {
        int luckScore = RNGController.GetDealerRNG(PlayerManager.Instance.Stats.LuckLevel * 5, 101);
        if (luckScore >= 70)
        {
            RoomCardEnchantment enchantment = _possibleEnchantments[RNGController.GetDealerRNG(0, _possibleEnchantments.Length)];
            card.Enchantments.Add(enchantment);
        }
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
        if (!ChipsManager.Instance.DecreaseChips(_cost)) return;

        _cardOptions = GenerateCardChoices(GenerateCardRarities());
        for (int i = 0; i < _cardOptionsSprites.Length; i++)
        {
            if (_cardOptions[i] == null) continue;

            _cardOptionsSprites[i].sprite = _cardOptions[i].Sprite;
            if (_cardOptions[i].Enchantments.Count > 0 && _cardOptions[i].Enchantments[0] == Greedy)
            {
                GameObject enchantIcon = Instantiate(_cardEnchantmentIconPrefab, _cardOptionsSprites[i].gameObject.transform);
                enchantIcon.transform.localScale = new Vector2(16,16);
                enchantIcon.GetComponent<SpriteRenderer>().sortingOrder = 2;
                _cardEnchantmentIcons.Add(enchantIcon);
            }
        }

        _hasRolledCards = true;
        _rollCardsButton.SetActive(false);
    }

    public void SelectCard(int index)
    {
        if (!_hasRolledCards) return;
        
        RoomCardData card = _cardOptions[index];

        RoomsDeckManager.Instance.AddCardToDeck(card);
        
        HasChosenCard = true;
        DealerShopManager.Instance.Close();
    }

    private void SetCost()
    {
        _cost = BASE_COST * FloorManager.Instance.CurrentFloor;
        _rollCardsCostText.text = $"<color=#FFFF00>Cost: {_cost}</color>";
    }

    public void OpenMenu()
    {
        SetCost();

        _uiObject.SetActive(true);
        _rollCardsButton.SetActive(!_hasRolledCards);
    }

    private void Awake()
    {
        InitializeAllCardRarities();
        ResetCardRolls();
    }
}
