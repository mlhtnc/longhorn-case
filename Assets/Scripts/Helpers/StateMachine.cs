using System;
using System.Collections.Generic;

// Notes
// 1. What a finite state machine is
// 2. Examples where you'd use one
//     AI, Animation, Game State
// 3. Parts of a State Machine
//     States & Transitions
// 4. States - 3 Parts
//     Tick - Why it's not Update()
//     OnEnter / OnExit (setup & cleanup)
// 5. Transitions
//     Separated from states so they can be re-used
//     Easy transitions from any state
public interface IState
{
    int Id { get; }
    void OnEnter();
    void OnExit();
}

public interface IStateTickable
{
    void Tick(float deltaTime);
}

public interface IStateFixedTickable
{
    void FixedTick(float deltaTime);
}

public class StateMachine
{
    private IState _currentState;

    private Dictionary<int, List<Transition>> _transitions = new Dictionary<int, List<Transition>>();
    private List<Transition> _currentTransitions = new List<Transition>();
    private List<Transition> _anyTransitions = new List<Transition>();

    private static List<Transition> EmptyTransitions = new List<Transition>(0);

    public IState CurrentState { get => _currentState; }

    public void Tick(float deltaTime)
    {
        var transition = GetTransition();
        if (transition != null)
            SetState(transition.To);

        if(_currentState is IStateTickable)
            ((IStateTickable) _currentState)?.Tick(deltaTime);
    }

    public void FixedTick(float deltaTime)
    {
        if(_currentState is IStateFixedTickable)
            ((IStateFixedTickable) _currentState).FixedTick(deltaTime);
    }

    public void SetState(IState state)
    {
        if (state == _currentState)
            return;

        _currentState?.OnExit();
        _currentState = state;

        _transitions.TryGetValue(_currentState.Id, out _currentTransitions);
        if (_currentTransitions == null)
            _currentTransitions = EmptyTransitions;

        _currentState.OnEnter();
    }

    public void AddTransition(IState from, IState to, Func<bool> predicate)
    {
        if (_transitions.TryGetValue(from.Id, out var transitions) == false)
        {
            transitions = new List<Transition>();
            _transitions[from.Id] = transitions;
        }

        transitions.Add(new Transition(to, predicate));
    }

    public void AddAnyTransition(IState state, Func<bool> predicate)
    {
        _anyTransitions.Add(new Transition(state, predicate));
    }

    private class Transition
    {
        public Func<bool> Condition { get; }
        public IState To { get; }

        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    private Transition GetTransition()
    {
        foreach (var transition in _anyTransitions)
            if (transition.Condition())
                return transition;

        foreach (var transition in _currentTransitions)
            if (transition.Condition())
                return transition;

        return null;
    }
}