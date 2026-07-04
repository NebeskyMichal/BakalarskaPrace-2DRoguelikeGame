using System.Collections.Generic;
using UnityEngine;

public class CardPoolSystem : Singleton<CardPoolSystem>
{
    [SerializeField] public List<CardData> allCardsInGame = new();

    public List<CardData> GetRandomCards(int amountToGenerate)
    {
        List<CardData> offeredCards = new();
        List<CardData> availableCards = new(allCardsInGame);
        int actualAmount = Mathf.Min(amountToGenerate, availableCards.Count);

        for (int i = 0; i < actualAmount; i++)
        {
            int randomIndex = Random.Range(0, availableCards.Count);
            offeredCards.Add(availableCards[randomIndex]);
            availableCards.RemoveAt(randomIndex);
        }

        return offeredCards;
    }
}