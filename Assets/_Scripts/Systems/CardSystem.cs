using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandView handView;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;
    [SerializeField] private CardViewCreator cardViewCreator;
    [SerializeField] private Transform playerHand;

    private readonly List<Card> _drawPile = new();
    private readonly List<Card> _discardPile = new();
    private readonly List<Card> _hand = new();

    void OnEnable()
    {
        ActionSystem.Instance.AttachPerformer<DrawCardsGameAction>(DrawCardsPerformer);
        ActionSystem.Instance.AttachPerformer<DiscardAllCardsGameAction>(DiscardCardsPerformer);
        ActionSystem.Instance.AttachPerformer<PlayCardGameAction>(PlayCardPerformer);
    }

    void OnDisable()
    {
        if (ActionSystem.Instance == null) return;
        ActionSystem.Instance.DetachPerformer<DrawCardsGameAction>();
        ActionSystem.Instance.DetachPerformer<DiscardAllCardsGameAction>();
        ActionSystem.Instance.DetachPerformer<PlayCardGameAction>();
    }

    public void Setup(List<CardData> deckData)
    {
        foreach (var cardData in deckData)
        {
            Card card = new(cardData);
            _drawPile.Add(card);
        }
    }

    private IEnumerator DrawCardsPerformer(DrawCardsGameAction drawCardsGameAction)
    {
        int actualAmount = Mathf.Min(drawCardsGameAction.Amount, _drawPile.Count);
        int notDrawn = drawCardsGameAction.Amount - actualAmount;

        for (int i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }

        if (notDrawn > 0)
        {
            RefillDeck();
            int reshuffledAmount = Mathf.Min(notDrawn, _drawPile.Count);

            for (int i = 0; i < reshuffledAmount; i++)
            {
                yield return DrawCard();
            }
        }
    }

    private IEnumerator DiscardCardsPerformer(DiscardAllCardsGameAction discardAllCardsGameAction)
    {
        foreach (var card in _hand)
        {
            CardView cardView = handView.RemoveCard(card);
            yield return DiscardCard(cardView);
        }

        _hand.Clear();
    }

    private IEnumerator PlayCardPerformer(PlayCardGameAction playCardGameAction)
    {
        _hand.Remove(playCardGameAction.Card);
        CardView cardView = handView.RemoveCard(playCardGameAction.Card);
        yield return DiscardCard(cardView);

        SpendManaGameAction spendManaGameAction = new(playCardGameAction.Card.Mana);
        ActionSystem.Instance.AddReaction(spendManaGameAction);

        if (playCardGameAction.Card.ManualTargetEffect != null)
        {
            PerformEffectGameAction performEffectGameAction = new(playCardGameAction.Card.ManualTargetEffect,
                new() { playCardGameAction.ManualTarget });
            ActionSystem.Instance.AddReaction(performEffectGameAction);
        }

        foreach (var effectWrapper in playCardGameAction.Card.OtherEffects)
        {
            List<CombatantView> targets = effectWrapper.TargetMode.GetTargets();
            PerformEffectGameAction performEffectGameAction = new(effectWrapper.Effect, targets);
            ActionSystem.Instance.AddReaction(performEffectGameAction);
        }
    }

    private IEnumerator DrawCard()
    {
        Card card = _drawPile.Draw();
        _hand.Add(card);
        CardView cardView =
            cardViewCreator.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation, playerHand);
        yield return handView.AddCard(cardView);
    }

    private void RefillDeck()
    {
        _drawPile.AddRange(_discardPile);
        _discardPile.Clear();

        for (int i = 0; i < _drawPile.Count; i++)
        {
            int randomIndex = Random.Range(i, _drawPile.Count);
            (_drawPile[i], _drawPile[randomIndex]) = (_drawPile[randomIndex], _drawPile[i]);
        }
    }

    private IEnumerator DiscardCard(CardView cardView)
    {
        _discardPile.Add(cardView.Card);
        cardView.transform.DOScale(Vector3.zero, 0.15f).SetLink(cardView.gameObject);
        Tween tween = cardView.transform.DOMove(discardPilePoint.position, 0.15f).SetLink(cardView.gameObject);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }
}