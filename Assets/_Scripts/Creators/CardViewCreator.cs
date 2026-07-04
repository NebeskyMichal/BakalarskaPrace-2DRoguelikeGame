using DG.Tweening;
using UnityEngine;

public class CardViewCreator : MonoBehaviour
{
    [SerializeField] private CardView cardViewPrefab;
    [SerializeField] private float drawAnimationDuration = 0.15f;

    public CardView CreateCardView(Card card, Vector3 position, Quaternion rotation, Transform playerHand)
    {
        CardView cardView = Instantiate(cardViewPrefab, position, rotation, playerHand);
        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(Vector3.one, drawAnimationDuration).SetLink(cardView.gameObject);
        cardView.Setup(card);
        return cardView;
    }
}