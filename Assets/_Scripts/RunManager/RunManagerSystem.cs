using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunManagerSystem : PersistentSingleton<RunManagerSystem>
{
    public PlayerRunData PlayerData { get; private set; }
    public PlayerRunStatistics PlayerRunStatistics { get; private set; }
    public EncounterData CurrentEncounter { get; private set; }
    public MapNode CurrentNode { get; set; }
    public bool IsDeckViewOpen { get; set; }
    public bool AreSettingsOpen { get; set; }

    public void StartRun(HeroData chosenHero, string seedText)
    {
        
        PlayerData = new PlayerRunData(chosenHero);
        PlayerRunStatistics = new PlayerRunStatistics();
        if (!string.IsNullOrEmpty(seedText))
        {
            PlayerData.SetSeed(seedText.GetHashCode());
        }
        else
        {
            PlayerData.SetSeed(Random.Range(100000, 9999999));
        }

        SceneManager.LoadScene(1);
    }

    public void StartEncounter(MapNode clickedNode)
    {
        CurrentNode = clickedNode;
        LegendUI.Instance.HideLegend();
        switch (clickedNode.EncounterData.EncounterType)
        {
            case EncounterType.Rest:
                RestingPlaceSystem.Instance.HealHero(PlayerData);
                PlayerData.UpdateFloor(PlayerData.CurrentFloor + 1);
                LegendUI.Instance.ShowLegend();
                break;
            case EncounterType.Shop:
                ShopSystem.Instance.OpenShop();
                PlayerData.UpdateFloor(PlayerData.CurrentFloor + 1);
                break;
            default:
            {
                CurrentEncounter = clickedNode.EncounterData;
                if (MapManagerSystem.Instance != null)
                {
                    MapManagerSystem.Instance.HideMap();
                }

                SceneManager.LoadScene(2, LoadSceneMode.Additive);
                break;
            }
        }
    }

    public void EndEncounter(bool playerWon)
    {
        if (playerWon)
        {
            PlayerData.UpdateFloor(PlayerData.CurrentFloor + 1);
            int goldReward = CurrentEncounter.GoldReward;
            PlayerData.GetGold(goldReward);
            PlayerRunStatistics.RecordGold(goldReward);
            StartCoroutine(UnloadBattleAndWakeMapRoutine());
            if (CurrentEncounter.EncounterType == EncounterType.Boss)
            {
                GameOverSystem.Instance.OpenGameOver(true, PlayerData, PlayerRunStatistics);
            }
            else
            {
                CurrentEncounter = null;
                RewardSystem.Instance.OpenRewardScreen(goldReward);
            }
        }
        else
        {
            StartCoroutine(UnloadBattleAndWakeMapRoutine());
            GameOverSystem.Instance.OpenGameOver(false, PlayerData, PlayerRunStatistics);
        }
    }

    private IEnumerator UnloadBattleAndWakeMapRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        yield return SceneManager.UnloadSceneAsync(2);
        if (MapManagerSystem.Instance != null)
        {
            MapManagerSystem.Instance.ShowMap();
        }
    }

    public void AddCardToDeck(CardData card)
    {
        PlayerData.PlayerDeck.Add(card);
    }

    public void ReturnToMenu()
    {
        PlayerData = null;
        PlayerRunStatistics = null;
        CurrentEncounter = null;
        CurrentNode = null;
        IsDeckViewOpen = false;
        AreSettingsOpen = false;
        SceneManager.LoadScene(0);
    }
}