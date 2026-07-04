using System.Collections;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private GameObject damageVFX;


    private void OnEnable()
    {
        ActionSystem.Instance.AttachPerformer<DealDamageGameAction>(DealDamagePerformer);
    }

    private void OnDisable()
    {
        if (ActionSystem.Instance == null) return;
        ActionSystem.Instance.DetachPerformer<DealDamageGameAction>();
    }


    private IEnumerator DealDamagePerformer(DealDamageGameAction dealDamageGameAction)
    {
        foreach (var target in dealDamageGameAction.Targets)
        {
            target.Damage(dealDamageGameAction.Amount);

            if (target is EnemyView)
            {
                RunManagerSystem.Instance.PlayerRunStatistics.RecordDamage(dealDamageGameAction.Amount);
            }

            Instantiate(damageVFX, target.transform.position, Quaternion.identity);

            if (target is HeroView)
            {
                RunManagerSystem.Instance.PlayerData.UpdateCurrentHealth(target.CurrentHealth);
            }

            yield return new WaitForSeconds(0.15f);
        }
    }
}