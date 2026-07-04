using UnityEngine;

public class KillEnemyGameAction : GameAction
{

    public EnemyView EnemyView { get; private set; }

    public KillEnemyGameAction(EnemyView enemyView)
    {
        EnemyView = enemyView;
    }
    
}
