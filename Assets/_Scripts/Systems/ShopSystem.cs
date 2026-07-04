using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : Singleton<ShopSystem>
{
    [SerializeField] private CardShopView cardPrefab;
    [SerializeField] private Transform shopLayoutParent;
    [SerializeField] private GameObject shopUIPanel;

    private void GenerateShopOfferings()
    {
        foreach (Transform child in shopLayoutParent)
        {
            Destroy(child.gameObject);
        }

        List<CardData> shopCards = CardPoolSystem.Instance.GetRandomCards(3);

        int basePrice = 30;
        int floorScaling = 10;
        int manaScaling = 20;

        foreach (var cardData in shopCards)
        {
            CardShopView newlySpawnedCard = Instantiate(cardPrefab, shopLayoutParent);
            newlySpawnedCard.Setup(cardData,
                basePrice + (RunManagerSystem.Instance.PlayerData.CurrentFloor * floorScaling) +
                (cardData.Mana * manaScaling));
        }
    }

    public void OpenShop()
    {
        shopUIPanel.SetActive(true);
        GenerateShopOfferings();
    }

    public void ExitShop()
    {
        shopUIPanel.SetActive(false);
        LegendUI.Instance.ShowLegend();
    }
}