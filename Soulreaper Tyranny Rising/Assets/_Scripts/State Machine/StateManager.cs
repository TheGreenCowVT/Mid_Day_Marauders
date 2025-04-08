using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> _states = new Dictionary<EState, BaseState<EState>>();
    
    protected BaseState<EState> _currentState;
    public EState _state;

    protected bool _isTransitioning = false;
    protected bool _isInitialized = false;
    
    void Start()
    {
        _currentState.EnterState();
    }

    public virtual void Update()
    {
        if (!_isInitialized) return;
        EState nextStateKey = _currentState.GetNextState();
        if (!_isTransitioning && nextStateKey.Equals(_currentState.StateKey))
        {
            _currentState.UpdateState();
        }
        else
        {
            TransistionToState(nextStateKey);
        }
    }

    public void TransistionToState(EState nextState)
    {
        if (!_isInitialized) return;
        
        _isTransitioning = true;
        _currentState.ExitState();
        _currentState = _states[nextState];
        _state = nextState; // So we can see this in the editor
        _currentState.EnterState();
        _isTransitioning = false;
    }
}
