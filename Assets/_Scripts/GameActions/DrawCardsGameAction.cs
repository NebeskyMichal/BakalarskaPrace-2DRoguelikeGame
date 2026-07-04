public class DrawCardsGameAction : GameAction
{
    public int Amount { get; private set; }

    public DrawCardsGameAction(int amount)
    {
        Amount = amount;
    }
}