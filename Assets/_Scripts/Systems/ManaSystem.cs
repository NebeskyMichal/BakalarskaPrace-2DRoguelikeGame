using System.Collections;
using UnityEngine;

public class ManaSystem : Singleton<ManaSystem>
{
    [SerializeField] private ManaUI manaUI;

    private const int MAX_MANA = 4;

    private int _currentMana = MAX_MANA;

    private void OnEnable()
    {
        ActionSystem.Instance.AttachPerformer<SpendManaGameAction>(SpendManaPerformer);
        ActionSystem.Instance.AttachPerformer<RefillManaGameAction>(RefillManaPerformer);
        ActionSystem.Instance.SubscribeReaction<EnemyTurnGameAction>(EnemyTurnPostReaction, ReactionTiming.Post);
    }

    private void OnDisable()
    {
        if (ActionSystem.Instance == null) return;
        ActionSystem.Instance.DetachPerformer<SpendManaGameAction>();
        ActionSystem.Instance.DetachPerformer<RefillManaGameAction>();
        ActionSystem.Instance.UnsubscribeReaction<EnemyTurnGameAction>(EnemyTurnPostReaction, ReactionTiming.Post);
    }


    public bool HasEnoughMana(int mana)
    {
        return _currentMana >= mana;
    }


    private IEnumerator SpendManaPerformer(SpendManaGameAction spendManaGameAction)
    {
        _currentMana -= spendManaGameAction.Amount;
        manaUI.UpdateManaText(_currentMana);
        yield return null;
    }

    private IEnumerator RefillManaPerformer(RefillManaGameAction refillManaGameAction)
    {
        _currentMana = MAX_MANA;
        manaUI.UpdateManaText(_currentMana);
        yield return null;
    }

    private void EnemyTurnPostReaction(EnemyTurnGameAction enemyTurnGameAction)
    {
        RefillManaGameAction refillManaGameAction = new();
        ActionSystem.Instance.AddReaction(refillManaGameAction);
    }
}