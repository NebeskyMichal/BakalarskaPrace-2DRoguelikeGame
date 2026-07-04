using System.Collections.Generic;
using UnityEngine;

public class EnemyView : CombatantView
{
    [SerializeField] private IntentUI intentUI;
    public int AttackPower { get; private set; }
    public int HealAmount { get; private set; }
    public int DefendAmount { get; private set; }
    public int DebuffAmount { get; private set; }
    public IntentType CurrentIntent { get; set; }
    public Dictionary<IntentType, int> PossibleIntents { get; private set; }
    public IntentUI IntentUI => intentUI;

    public void Setup(EnemyData enemyData)
    {
        AttackPower = enemyData.AttackPower;
        HealAmount = enemyData.HealAmount;
        DefendAmount = enemyData.DefendAmount;
        DebuffAmount = enemyData.DebuffAmount;
        PossibleIntents = new Dictionary<IntentType, int>();
        foreach (var item in enemyData.PossibleIntents)
        {
            if (!PossibleIntents.ContainsKey(item.intent))
            {
                PossibleIntents.Add(item.intent, item.chancePercentage);
            }
        }

        SetupBase(enemyData.Health, enemyData.Health, enemyData.Image);
    }
}