using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : Singleton<EnemySystem>
{
    [SerializeField] private EnemyBoardView enemyBoardView;
    public List<EnemyView> Enemies => enemyBoardView.EnemyViews;

    private void OnEnable()
    {
        ActionSystem.Instance.AttachPerformer<EnemyTurnGameAction>(EnemyTurnPerformer);
        ActionSystem.Instance.AttachPerformer<AttackHeroGameAction>(AttackHeroPerformer);
        ActionSystem.Instance.AttachPerformer<KillEnemyGameAction>(KillEnemyPerformer);
        ActionSystem.Instance.AttachPerformer<UpdateIntentGameAction>(UpdateIntentPerformer);
    }

    private void OnDisable()
    {
        if (ActionSystem.Instance == null) return;
        ActionSystem.Instance.DetachPerformer<EnemyTurnGameAction>();
        ActionSystem.Instance.DetachPerformer<AttackHeroGameAction>();
        ActionSystem.Instance.DetachPerformer<KillEnemyGameAction>();
        ActionSystem.Instance.DetachPerformer<UpdateIntentGameAction>();
    }

    public void Setup(List<EnemyData> enemyDatas)
    {
        foreach (var enemyData in enemyDatas)
        {
            enemyBoardView.AddEnemy(enemyData);
        }
    }

    private IEnumerator EnemyTurnPerformer(EnemyTurnGameAction enemyTurnGameAction)
    {
        foreach (var enemy in enemyBoardView.EnemyViews)
        {
            int burnStacks = enemy.GetStatusEffectStacks(StatusEffectType.Burn);
            if (burnStacks > 0)
            {
                ApplyBurnGameAction burnDamageGameAction = new(enemy, burnStacks);
                ActionSystem.Instance.AddReaction(burnDamageGameAction);
            }

            switch (enemy.CurrentIntent)
            {
                case IntentType.Attack:
                    AttackHeroGameAction attackHeroGameAction = new(enemy);
                    ActionSystem.Instance.AddReaction(attackHeroGameAction);
                    enemy.CurrentScalingCounter++;
                    break;
                case IntentType.Heal:
                    HealGameAction healGameAction = new(enemy.HealAmount,
                        new List<CombatantView>(enemyBoardView.EnemyViews), enemy);
                    ActionSystem.Instance.AddReaction(healGameAction);
                    enemy.CurrentScalingCounter++;
                    break;
                case IntentType.Defend:
                    AddStatusEffectGameAction addStatusEffectGameAction =
                        new AddStatusEffectGameAction(StatusEffectType.Armor, enemy.DefendAmount,
                            new List<CombatantView>() { enemy });
                    ActionSystem.Instance.AddReaction(addStatusEffectGameAction);
                    enemy.CurrentScalingCounter++;
                    break;
                case IntentType.Debuff:
                    AddStatusEffectGameAction addStatusEffectEffectGameAction =
                        new AddStatusEffectGameAction(StatusEffectType.Burn, enemy.DebuffAmount,
                            new List<CombatantView>() { HeroSystem.Instance.HeroView });
                    ActionSystem.Instance.AddReaction(addStatusEffectEffectGameAction);
                    enemy.CurrentScalingCounter++;
                    break;
            }

            if (enemy.CurrentScalingCounter == 2)
            {
                IncreaseDamageGameAction increaseDamageGameAction = new(2, enemy);
                ActionSystem.Instance.AddReaction(increaseDamageGameAction);
                enemy.CurrentScalingCounter = 0;
            }

            UpdateIntentGameAction updateIntentAction = new(enemy);
            ActionSystem.Instance.AddReaction(updateIntentAction);
        }

        yield return null;
    }

    public IntentType PrepareNextAction(EnemyView enemy)
    {
        IntentType lastMove = enemy.CurrentIntent;
        int totalValidWeight = 0;

        foreach (var intent in enemy.PossibleIntents)
        {
            if (intent.Key == lastMove && intent.Key != IntentType.Attack && enemy.PossibleIntents.Count > 1)
            {
                continue;
            }

            if (intent.Key == IntentType.Heal && enemy.CurrentHealth >= enemy.MaxHealth)
            {
                continue;
            }

            totalValidWeight += intent.Value;
        }

        if (totalValidWeight <= 0)
        {
            enemy.CurrentIntent = IntentType.Attack;
            if (enemy.IntentUI != null)
            {
                enemy.IntentUI.Set(enemy.CurrentIntent, enemy.AttackPower);
            }

            return enemy.CurrentIntent;
        }

        int randomValue = Random.Range(0, totalValidWeight);
        int cumulativeChance = 0;

        foreach (var intent in enemy.PossibleIntents)
        {
            if (intent.Key == lastMove && intent.Key != IntentType.Attack && enemy.PossibleIntents.Count > 1)
            {
                continue;
            }

            if (intent.Key == IntentType.Heal && enemy.CurrentHealth >= enemy.MaxHealth)
            {
                continue;
            }

            cumulativeChance += intent.Value;

            if (randomValue < cumulativeChance)
            {
                enemy.CurrentIntent = intent.Key;

                int valueToShow = intent.Key switch
                {
                    IntentType.Attack => enemy.AttackPower + enemy.CurrentBonusDamage,
                    IntentType.Heal => enemy.HealAmount,
                    IntentType.Defend => enemy.DefendAmount,
                    IntentType.Debuff => enemy.DebuffAmount,
                    _ => 0
                };
                enemy.IntentUI.Set(enemy.CurrentIntent, valueToShow);

                return enemy.CurrentIntent;
            }
        }

        enemy.CurrentIntent = IntentType.Attack;
        enemy.IntentUI.Set(enemy.CurrentIntent, enemy.AttackPower + enemy.CurrentBonusDamage);
        return enemy.CurrentIntent;
    }

    private IEnumerator AttackHeroPerformer(AttackHeroGameAction attackHeroGameAction)
    {
        if (attackHeroGameAction.Caster == null || attackHeroGameAction.Caster.CurrentHealth <= 0)
        {
            yield break;
        }

        EnemyView attacker = (EnemyView)attackHeroGameAction.Caster;
        Tween tween = attacker.transform.DOMoveX(attacker.transform.position.x - 1f, 0.15f);
        yield return tween.WaitForCompletion();

        attacker.transform.DOMoveX(attacker.transform.position.x + 1f, 0.25f);

        DealDamageGameAction dealDamageGameAction = new(
            attacker.AttackPower + attacker.CurrentBonusDamage,
            new List<CombatantView> { HeroSystem.Instance.HeroView },
            attackHeroGameAction.Caster
        );

        ActionSystem.Instance.AddReaction(dealDamageGameAction);
    }

    private IEnumerator KillEnemyPerformer(KillEnemyGameAction killEnemyGameAction)
    {
        yield return enemyBoardView.RemoveEnemy(killEnemyGameAction.EnemyView);

        if (killEnemyGameAction.EnemyView != null)
        {
            killEnemyGameAction.EnemyView.transform.DOKill(true);

            Destroy(killEnemyGameAction.EnemyView.gameObject);
        }
    }

    private IEnumerator UpdateIntentPerformer(UpdateIntentGameAction updateIntentGameAction)
    {
        if (updateIntentGameAction.EnemyView == null || updateIntentGameAction.EnemyView.CurrentHealth <= 0)
        {
            yield break;
        }

        PrepareNextAction(updateIntentGameAction.EnemyView);
        yield return new WaitForSeconds(0.2f);
    }
}