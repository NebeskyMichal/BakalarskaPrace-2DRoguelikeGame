using UnityEngine;

public class EnemyViewCreator : MonoBehaviour
{
    [SerializeField] private EnemyView enemyViewPrefab;

    public EnemyView CreateEnemyView(EnemyData enemyData, Vector3 position, Quaternion rotation, Transform slot)
    {
        EnemyView enemyView = Instantiate(enemyViewPrefab, position, rotation, slot);
        enemyView.Setup(enemyData);
        return enemyView;
    }
}
