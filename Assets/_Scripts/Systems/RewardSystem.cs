using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardSystem : Singleton<RewardSystem>
{
    [SerializeField] private CardRewardView cardPrefab;
    [SerializeField] private Transform cardRewardParent;
    [SerializeField] private GameObject rewardUiPanel;
    [SerializeField] private TMP_Text rewardGoldText;

    private void OnEnable()
    {
        CardRewardView.onRewardCardSelected += HandleCardChosen;
    }

    private void OnDisable()
    {
        CardRewardView.onRewardCardSelected -= HandleCardChosen;
    }

    private void GenerateRewards(int goldReward)
    {
        foreach (Transform child in cardRewardParent)
        {
            Destroy(child.gameObject);
        }

        List<CardData> rewardCards = CardPoolSystem.Instance.GetRandomCards(3);

        foreach (var cardData in rewardCards)
        {
            CardRewardView newlySpawnedCard = Instantiate(cardPrefab, cardRewardParent);
            newlySpawnedCard.Setup(cardData);
        }

        rewardGoldText.text = $"+ {goldReward.ToString()}";
    }

    public void OpenRewardScreen(int goldReward)
    {
        rewardUiPanel.SetActive(true);
        GenerateRewards(goldReward);
    }

    private void HandleCardChosen(CardData chosenCard)
    {
        rewardUiPanel.SetActive(false);
        LeaveRewardScreen();
    }

    private void LeaveRewardScreen()
    {
        rewardUiPanel.SetActive(false);
        LegendUI.Instance.ShowLegend();
    }
}