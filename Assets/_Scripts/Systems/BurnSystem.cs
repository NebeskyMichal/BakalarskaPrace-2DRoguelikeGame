using System.Collections;
using UnityEngine;

public class BurnSystem : MonoBehaviour
{
    [SerializeField] private GameObject vfx;

    private void OnEnable()
    {
        ActionSystem.Instance.AttachPerformer<ApplyBurnGameAction>(ApplyBurnPerformer);
    }

    private void OnDisable()
    {
        if (ActionSystem.Instance == null) return;
        ActionSystem.Instance.DetachPerformer<ApplyBurnGameAction>();
    }

    private IEnumerator ApplyBurnPerformer(ApplyBurnGameAction applyBurnGameAction)
    {
        CombatantView target = applyBurnGameAction.Target;

        if (target == null || target.CurrentHealth <= 0)
        {
            yield break;
        }

        Instantiate(vfx, target.transform.position, Quaternion.identity);

        target.Damage(applyBurnGameAction.BurnDamage);

        if (target is EnemyView)
        {
            RunManagerSystem.Instance.PlayerRunStatistics.RecordDamage(applyBurnGameAction.BurnDamage);
        }

        target.RemoveStatusEffect(StatusEffectType.Burn, 1);

        yield return new WaitForSeconds(1f);
    }
}