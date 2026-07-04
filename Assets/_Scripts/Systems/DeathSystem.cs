using UnityEngine;
using System.Collections;


public class DeathSystem : MonoBehaviour
{
    private void OnEnable()
    {
        ActionSystem.Instance.AttachPerformer<CombatantDeathGameAction>(CombatantDiedPerformer);
    }

    private void OnDisable()
    {
        if (ActionSystem.Instance == null) return;
        ActionSystem.Instance.DetachPerformer<CombatantDeathGameAction>();
    }

    private IEnumerator CombatantDiedPerformer(CombatantDeathGameAction deathAction)
    {
        CombatantView deadTarget = deathAction.DeadCombatant;

        if (deadTarget is EnemyView enemyView)
        {
            RunManagerSystem.Instance.PlayerRunStatistics.AddOneEnemy();
            KillEnemyGameAction killEnemyGameAction = new(enemyView);
            ActionSystem.Instance.AddReaction(killEnemyGameAction);
            yield return new WaitForSeconds(0.5f);

            bool isListEmpty = EnemySystem.Instance.Enemies.Count == 0;

            bool isOnlyDyingEnemyLeft = EnemySystem.Instance.Enemies.Count == 1 &&
                                        EnemySystem.Instance.Enemies.Contains(enemyView);

            if (isListEmpty || isOnlyDyingEnemyLeft)
            {
                RunManagerSystem.Instance.EndEncounter(true);
            }
        }
        else if (deadTarget is HeroView)
        {
            yield return new WaitForSeconds(0.5f);
            RunManagerSystem.Instance.EndEncounter(false);
        }
    }
}