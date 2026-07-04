using UnityEngine;

public class AttackHeroGameAction : GameAction, IHaveCaster
{
    public CombatantView Caster { get; private set; }

    public AttackHeroGameAction(EnemyView attacker)
    {
        Caster = attacker;
    }
}