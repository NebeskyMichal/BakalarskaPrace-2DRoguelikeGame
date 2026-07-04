using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DealDamageEffect : Effect
{

    [SerializeField] private int damageAmount;

    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        DealDamageGameAction dealDamageGameAction = new(damageAmount, targets, caster);
        return dealDamageGameAction;
    }
   
}
