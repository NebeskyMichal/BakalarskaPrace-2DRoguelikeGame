using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    private void Start()
    {
        PlayerRunData livePlayerData = RunManagerSystem.Instance.PlayerData;
        EncounterData liveEncounter = RunManagerSystem.Instance.CurrentEncounter;

        if (liveEncounter == null || livePlayerData == null)
        {
            return;
        }

        EnemySystem.Instance.Setup(liveEncounter.EnemyDatas);
        HeroSystem.Instance.Setup(livePlayerData.ChosenHero, livePlayerData.CurrentHealth);
        CardSystem.Instance.Setup(livePlayerData.PlayerDeck);

        RefillManaGameAction refillManaGa = new();
        ActionSystem.Instance.Perform(refillManaGa, () =>
        {
            DrawCardsGameAction drawCardsGa = new(5);
            ActionSystem.Instance.Perform(drawCardsGa);
        });
    }
}