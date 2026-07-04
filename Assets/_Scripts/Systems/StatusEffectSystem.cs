using System.Collections;
using UnityEngine;

public class StatusEffectSystem : MonoBehaviour
{
    private void OnEnable()
    {
        ActionSystem.Instance.AttachPerformer<AddStatusEffectGameAction>(AddStatusEffectPerformer);
    }

    private void OnDisable()
    {
        if (ActionSystem.Instance == null) return;
        ActionSystem.Instance.DetachPerformer<AddStatusEffectGameAction>();
    }

    private IEnumerator AddStatusEffectPerformer(AddStatusEffectGameAction addStatusEffectGameAction)
    {
        foreach (var target in addStatusEffectGameAction.Targets)
        {
            if (target == null) 
            {
                continue; 
            }
            
            target.AddStatusEffect(addStatusEffectGameAction.StatusEffectType, addStatusEffectGameAction.StackCount);
            yield return null;
        }
    }
}