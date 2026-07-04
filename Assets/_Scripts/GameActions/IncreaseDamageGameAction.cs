public class IncreaseDamageGameAction : GameAction, IHaveCaster
{
    public int Amount { get; private set; }

    public CombatantView Caster { get; private set; }

    public IncreaseDamageGameAction(int amount, CombatantView caster)
    {
        Amount = amount;
        Caster = caster;
    }
}