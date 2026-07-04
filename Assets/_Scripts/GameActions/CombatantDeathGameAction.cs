public class CombatantDeathGameAction : GameAction
{
    public CombatantView DeadCombatant { get; }

    public CombatantDeathGameAction(CombatantView deadCombatant)
    {
        DeadCombatant = deadCombatant;
    }
}