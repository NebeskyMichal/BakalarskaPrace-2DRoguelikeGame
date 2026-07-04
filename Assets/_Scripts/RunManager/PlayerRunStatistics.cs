using UnityEngine;

[System.Serializable]
public class PlayerRunStatistics
{
    [SerializeField] private int enemiesKilled = 0;
    [SerializeField] private int damageDoneTotal = 0;
    [SerializeField] private int goldEarned = 0;

    public int EnemiesKilled => enemiesKilled;
    public int DamageDoneTotal => damageDoneTotal;
    public int GoldEarned => goldEarned;

    public void RecordGold(int amountToRecord)
    {
        goldEarned += amountToRecord;
    }

    public void AddOneEnemy()
    {
        enemiesKilled++;
    }

    public void RecordDamage(int amountToRecord)
    {
        damageDoneTotal += amountToRecord;
    }
}