using UnityEngine;

public class ApplyBurnGameAction : GameAction
{
    public int BurnDamage { get; private set; }

    public CombatantView Target { get; private set; }

    public ApplyBurnGameAction(CombatantView target, int burnDamage)
    {
        Target = target;
        BurnDamage = burnDamage;
    }

}
