using System;
using System.Collections;
using System.Collections.Generic;

public class ActionSystem : Singleton<ActionSystem>
{
    private List<GameAction> _currentReactionsList = null;
    public bool IsPerforming { get; private set; } = false;

    private Dictionary<Type, Dictionary<Delegate, Action<GameAction>>> _preSubs = new();
    private Dictionary<Type, Dictionary<Delegate, Action<GameAction>>> _postSubs = new();
    private Dictionary<Type, Func<GameAction, IEnumerator>> _performers = new();

    private void Start()
    {
        IsPerforming = false;
    }
    
    public void Perform(GameAction action, System.Action onPerformFinished = null)
    {
        if (IsPerforming) return;
        IsPerforming = true;
        StartCoroutine(Flow(action, () =>
        {
            IsPerforming = false;
            onPerformFinished?.Invoke();
        }));
    }

    public void AddReaction(GameAction gameAction)
    {
        _currentReactionsList?.Add(gameAction);
    }

    private IEnumerator Flow(GameAction action, Action onFlowFinished = null)
    {
        List<GameAction> previousReactionsList = _currentReactionsList;

        _currentReactionsList = action.PreReactions;
        PerformSubscribers(action, _preSubs);
        yield return PerformReactions(_currentReactionsList);

        _currentReactionsList = action.PerformReactions;
        yield return PerformPerformer(action);
        yield return PerformReactions(_currentReactionsList);

        _currentReactionsList = action.PostReactions;
        PerformSubscribers(action, _postSubs);
        yield return PerformReactions(_currentReactionsList);

        _currentReactionsList = previousReactionsList;

        onFlowFinished?.Invoke();
    }

    private IEnumerator PerformPerformer(GameAction action)
    {
        Type type = action.GetType();
        if (_performers.ContainsKey(type))
        {
            yield return _performers[type](action);
        }
    }

    private void PerformSubscribers(GameAction action, Dictionary<Type, Dictionary<Delegate, Action<GameAction>>> subs)
    {
        Type type = action.GetType();
        if (subs.ContainsKey(type))
        {
            foreach (var sub in subs[type].Values)
            {
                sub(action);
            }
        }
    }

    private IEnumerator PerformReactions(List<GameAction> reactionsToProcess)
    {
        if (reactionsToProcess == null) yield break;

        for (int i = 0; i < reactionsToProcess.Count; i++)
        {
            yield return Flow(reactionsToProcess[i]);
        }
    }

    public void AttachPerformer<T>(Func<T, IEnumerator> performer) where T : GameAction
    {
        Type type = typeof(T);
        IEnumerator WrappedPerformer(GameAction action) => performer((T)action);
        if (_performers.ContainsKey(type)) _performers[type] = WrappedPerformer;
        else _performers.Add(type, WrappedPerformer);
    }

    public void DetachPerformer<T>() where T : GameAction
    {
        Type type = typeof(T);
        if (_performers.ContainsKey(type)) _performers.Remove(type);
    }

    public void SubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        var subs = timing == ReactionTiming.Pre ? _preSubs : _postSubs;
        Type type = typeof(T);

        if (!subs.ContainsKey(type))
        {
            subs.Add(type, new Dictionary<Delegate, Action<GameAction>>());
        }

        if (!subs[type].ContainsKey(reaction))
        {
            Action<GameAction> wrappedReaction = (GameAction action) => reaction((T)action);
            subs[type].Add(reaction, wrappedReaction);
        }
    }

    public void UnsubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        var subs = timing == ReactionTiming.Pre ? _preSubs : _postSubs;
        Type type = typeof(T);

        if (subs.ContainsKey(type) && subs[type].ContainsKey(reaction))
        {
            subs[type].Remove(reaction);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _preSubs.Clear();
        _postSubs.Clear();
        _performers.Clear();
    }
}