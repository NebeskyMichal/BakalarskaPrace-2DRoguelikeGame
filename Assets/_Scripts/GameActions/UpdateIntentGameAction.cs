public class UpdateIntentGameAction : GameAction
{
    public EnemyView EnemyView { get; private set; }

    public UpdateIntentGameAction(EnemyView enemyView)
    {
        EnemyView = enemyView;
    }
}