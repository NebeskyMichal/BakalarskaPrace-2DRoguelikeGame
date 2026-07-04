public class SpendManaGameAction : GameAction
{
    public int Amount { get; private set; }

    public SpendManaGameAction(int amount)
    {
        Amount = amount;
    }
}