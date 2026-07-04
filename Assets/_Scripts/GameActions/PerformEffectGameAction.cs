using System.Collections.Generic;

public class PerformEffectGameAction : GameAction
{
    public Effect Effect { get; private set; }

    public List<CombatantView> Targets { get; private set; }

    public PerformEffectGameAction(Effect effect, List<CombatantView> targets)
    {
        Effect = effect;
        Targets = targets == null ? null : new(targets);
    }
}