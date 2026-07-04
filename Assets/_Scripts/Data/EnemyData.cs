using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy")]
public class EnemyData : ScriptableObject
{
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int AttackPower { get; private set; }
    [field: SerializeField] public int HealAmount { get; private set; }
    [field: SerializeField] public int DefendAmount { get; private set; }
    [field: SerializeField] public int DebuffAmount { get; private set; }
    [System.Serializable]
    public struct IntentChance
    {
        public IntentType intent;

        [Range(0, 100)] public int chancePercentage;
    }

    [field: SerializeField] public List<IntentChance> PossibleIntents { get; private set; }
}