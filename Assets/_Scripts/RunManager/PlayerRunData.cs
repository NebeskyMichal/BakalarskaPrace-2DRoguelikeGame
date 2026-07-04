using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerRunData
{
    [SerializeField] private HeroData chosenHero;
    [SerializeField] private List<CardData> playerDeck;
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentGold;
    [SerializeField] private int currentFloor;

    public string HeroName { get; private set; }
    public Sprite HeroPortrait { get; private set; }
    public int MapSeed { get; private set; }
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public int CurrentGold => currentGold;
    public int CurrentFloor => currentFloor;
    public HeroData ChosenHero => chosenHero;
    public List<CardData> PlayerDeck => playerDeck;


    public PlayerRunData(HeroData heroData)
    {
        chosenHero = heroData;
        playerDeck = new List<CardData>(heroData.Deck);
        maxHealth = heroData.Health;
        currentHealth = maxHealth;
        currentGold = 0;
        currentFloor = 1;
        HeroName = heroData.name;
        HeroPortrait = heroData.HeroPortrait;
    }

    public void GetGold(int amountToGet)
    {
        currentGold += amountToGet;
        TopBarManagerSystem.Instance.UpdateUI();
    }

    public bool SpendGold(int amountToSpend)
    {
        if (CurrentGold < amountToSpend) return false;
        currentGold -= amountToSpend;
        TopBarManagerSystem.Instance.UpdateUI();
        return true;
    }

    public void UpdateCurrentHealth(int newHealthValue)
    {
        currentHealth = Mathf.Clamp(newHealthValue, 0, maxHealth);
        TopBarManagerSystem.Instance.UpdateUI();
    }

    public void UpdateFloor(int floorNumber)
    {
        currentFloor = floorNumber;
        TopBarManagerSystem.Instance.UpdateUI();
    }

    public void SetSeed(int customSeed)
    {
        MapSeed = customSeed;
    }
}