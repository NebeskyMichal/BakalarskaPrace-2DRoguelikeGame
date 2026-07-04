using System.Collections;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    private void OnEnable()
    {
        ActionSystem.Instance.AttachPerformer<PerformEffectGameAction>(PerformEffectPerformer);
    }

    private void OnDisable()
    {
        if (ActionSystem.Instance == null) return;
        ActionSystem.Instance.DetachPerformer<PerformEffectGameAction>();
    }

    private IEnumerator PerformEffectPerformer(PerformEffectGameAction performEffectGameAction)
    {
        GameAction effectAction =
            performEffectGameAction.Effect.GetGameAction(performEffectGameAction.Targets, HeroSystem.Instance.HeroView);
        ActionSystem.Instance.AddReaction(effectAction);
        yield return null;
    }
}