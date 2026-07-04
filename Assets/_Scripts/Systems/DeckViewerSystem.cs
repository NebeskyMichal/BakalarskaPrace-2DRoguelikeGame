using UnityEngine;
using System.Collections.Generic;

public class DeckViewerSystem : MonoBehaviour
{
    [SerializeField] private GameObject deckViewerPanel;
    [SerializeField] private Transform gridLayoutParent;
    [SerializeField] private CardRewardView cardPrefab;

    public void OpenDeckViewer()
    {
        deckViewerPanel.SetActive(true);
        RunManagerSystem.Instance.IsDeckViewOpen = true;
        RefreshDeckDisplay();
        LegendUI.Instance.HideLegend();
    }

    public void CloseDeckViewer()
    {
        RunManagerSystem.Instance.IsDeckViewOpen = false;
        deckViewerPanel.SetActive(false);
        if (RunManagerSystem.Instance.CurrentEncounter == null)
        {
            LegendUI.Instance.ShowLegend();   
        }
    }

    private void RefreshDeckDisplay()
    {
        foreach (Transform child in gridLayoutParent)
        {
            Destroy(child.gameObject);
        }

        List<CardData> masterDeck = RunManagerSystem.Instance.PlayerData.PlayerDeck;

        foreach (CardData card in masterDeck)
        {
            CardRewardView spawnedCard = Instantiate(cardPrefab, gridLayoutParent);
            spawnedCard.Setup(card);
        }
    }
}