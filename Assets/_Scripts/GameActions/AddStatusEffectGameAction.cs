using System.Collections.Generic;
using UnityEngine;

public class AddStatusEffectGameAction : GameAction
{
    public StatusEffectType StatusEffectType {get; private set;}
    public int StackCount {get; private set;}
    public List<CombatantView> Targets {get; private set;}

    public AddStatusEffectGameAction(StatusEffectType statusEffectType, int stackCount, List<CombatantView> targets)
    {
        StatusEffectType = statusEffectType;
        StackCount = stackCount;
        Targets = targets;
    }
}
