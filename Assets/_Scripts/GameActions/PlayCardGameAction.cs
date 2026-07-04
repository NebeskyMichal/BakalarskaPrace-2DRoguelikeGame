public class PlayCardGameAction : GameAction
{
    public Card Card { get; private set; }

    public EnemyView ManualTarget { get; private set; } = null;

    public PlayCardGameAction(Card card, EnemyView manualTarget)
    {
        Card = card;
        ManualTarget = manualTarget;
    }
}