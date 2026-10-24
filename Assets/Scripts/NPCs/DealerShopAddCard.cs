using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Rarity;

public class DealerShopAddCard : MonoBehaviour
{
    private Dictionary<Rarity, int> _rarityWeights = new Dictionary<Rarity, int>{{Common, 30}, {Uncommon, 20}, {Rare, 7}, {Epic, 3}, {Legendary, 1}};

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

        foreach (Rarity rarity in offeredRarities) Debug.Log(rarity);

        return offeredRarities;
    }

    public void RollCards()
    {
        GenerateCardRarities();
    }
}
