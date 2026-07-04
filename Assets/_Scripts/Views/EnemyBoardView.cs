using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;

public class EnemyBoardView : MonoBehaviour
{
    [SerializeField] private List<Transform> slots;
    [SerializeField] private EnemyViewCreator enemyViewCreator;

    public List<EnemyView> EnemyViews { get; private set; } = new();

    public void AddEnemy(EnemyData enemyData)
    {
        Transform slot = slots[EnemyViews.Count];
        EnemyView enemyView = enemyViewCreator.CreateEnemyView(enemyData, slot.position, slot.rotation, slot);
        EnemySystem.Instance.PrepareNextAction(enemyView);
        EnemyViews.Add(enemyView);
    }

    public IEnumerator RemoveEnemy(EnemyView enemyView)
    {
        if (enemyView == null) yield break;
        EnemyViews.Remove(enemyView);
        Tween tween = enemyView.transform.DOScale(Vector4.zero, 0.25f).SetLink(enemyView.gameObject);
        yield return tween.WaitForCompletion();
    }
}