using System.Collections;
using UnityEngine;

public class DamageIncreaseSystem : MonoBehaviour
{
    [SerializeField] private GameObject buffVFX;

    private void OnEnable()
    {
        ActionSystem.Instance.AttachPerformer<IncreaseDamageGameAction>(IncreaseDamagePerformer);
    }

    private void OnDisable()
    {
        if (ActionSystem.Instance == null) return;
        ActionSystem.Instance.DetachPerformer<IncreaseDamageGameAction>();
    }


    private IEnumerator IncreaseDamagePerformer(IncreaseDamageGameAction increaseDamageGameAction)
    {
        if (increaseDamageGameAction.Caster == null || increaseDamageGameAction.Caster.CurrentHealth <= 0)
        {
            yield break;
        }

        increaseDamageGameAction.Caster.CurrentBonusDamage += increaseDamageGameAction.Amount;
        var vfx = Instantiate(buffVFX, increaseDamageGameAction.Caster.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.15f);
    }
}