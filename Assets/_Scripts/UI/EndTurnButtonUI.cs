using UnityEngine;

public class EndTurnButtonUI : MonoBehaviour
{
    public void OnClick()
    {
        EnemyTurnGameAction enemyTurnGameAction = new();
        ActionSystem.Instance.Perform(enemyTurnGameAction);
    }
}