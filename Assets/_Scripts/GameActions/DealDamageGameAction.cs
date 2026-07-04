using System.Collections.Generic;

public class DealDamageGameAction : GameAction, IHaveCaster
{
    public int Amount { get; set; }

    public List<CombatantView> Targets { get; set; }

    public CombatantView Caster { get; private set; }

    public DealDamageGameAction(int amount, List<CombatantView> targets, CombatantView caster)
    {
        Amount = amount;
        Targets = new(targets);
        Caster = caster;
    }
}