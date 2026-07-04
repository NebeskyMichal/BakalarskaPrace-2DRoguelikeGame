using UnityEngine;

public class HeroSystem : Singleton<HeroSystem>
{
    public HeroView HeroView { get; private set; }
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private HeroViewCreator heroViewCreator;

    void OnEnable()
    {
        ActionSystem.Instance.SubscribeReaction<EnemyTurnGameAction>(EnemyTurnPreReaction, ReactionTiming.Pre);
        ActionSystem.Instance.SubscribeReaction<EnemyTurnGameAction>(EnemyTurnPostReaction, ReactionTiming.Post);
    }

    void OnDisable()
    {
        if (ActionSystem.Instance == null) return;
        ActionSystem.Instance.UnsubscribeReaction<EnemyTurnGameAction>(EnemyTurnPreReaction, ReactionTiming.Pre);
        ActionSystem.Instance.UnsubscribeReaction<EnemyTurnGameAction>(EnemyTurnPostReaction, ReactionTiming.Post);
    }

    private void EnemyTurnPreReaction(EnemyTurnGameAction enemyTurnGa)
    {
        DiscardAllCardsGameAction discardAllCardsGameAction = new();
        ActionSystem.Instance.AddReaction(discardAllCardsGameAction);
    }

    private void EnemyTurnPostReaction(EnemyTurnGameAction enemyTurnGa)
    {
        int burnStacks = HeroView.GetStatusEffectStacks(StatusEffectType.Burn);
        if (burnStacks > 0)
        {
            ApplyBurnGameAction burnDamageGameAction = new(HeroView, burnStacks);
            ActionSystem.Instance.AddReaction(burnDamageGameAction);
        }

        DrawCardsGameAction drawCardsGameAction = new(5);
        ActionSystem.Instance.AddReaction(drawCardsGameAction);
    }

    public void Setup(HeroData heroData, int currHealth)
    {
        HeroView = heroViewCreator.CreateHeroView(heroData, currHealth, spawnPoint.position, spawnPoint.rotation,
            spawnPoint);
    }
}