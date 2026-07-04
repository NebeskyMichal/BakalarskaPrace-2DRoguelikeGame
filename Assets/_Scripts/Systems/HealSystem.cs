using System.Collections;
using UnityEngine;

public class HealSystem : MonoBehaviour
{
    [SerializeField] private GameObject healVFX;

    private void OnEnable()
    {
        ActionSystem.Instance.AttachPerformer<HealGameAction>(HealPerformer);
    }

    private void OnDisable()
    {
        if (ActionSystem.Instance == null) return;
        ActionSystem.Instance.DetachPerformer<HealGameAction>();
    }


    private IEnumerator HealPerformer(HealGameAction healGameAction)
    {
        if (healGameAction.Caster.CurrentHealth <= 0)
        {
            yield break;
        }
        else
        {
            healGameAction.Caster.Heal(healGameAction.Amount);
            var vfx = Instantiate(healVFX, healGameAction.Caster.transform.position, Quaternion.identity);
            if (healGameAction.Caster is HeroView)
            {
                RunManagerSystem.Instance.PlayerData.UpdateCurrentHealth(healGameAction.Caster.CurrentHealth);
            }

            yield return new WaitForSeconds(0.15f);
        }
    }
}