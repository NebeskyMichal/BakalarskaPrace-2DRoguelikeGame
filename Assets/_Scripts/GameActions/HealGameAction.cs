using System.Collections.Generic;

public class HealGameAction : GameAction, IHaveCaster
{
    public int Amount { get; private set; }

    public List<CombatantView> Targets { get; private set; }

    public CombatantView Caster { get; private set; }

    public HealGameAction(int amount, List<CombatantView> targets, CombatantView caster)
    {
        Amount = amount;
        Targets = new(targets);
        Caster = caster;
    }
}