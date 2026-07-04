using System.Collections.Generic;

public abstract class GameAction
{
    public List<GameAction> PreReactions { get; private set; } = new();
    public List<GameAction> PerformReactions { get; protected set; } = new();
    public List<GameAction> PostReactions { get; private set; } = new();
}