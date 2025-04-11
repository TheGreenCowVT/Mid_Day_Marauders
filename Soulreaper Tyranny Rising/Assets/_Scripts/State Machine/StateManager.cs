using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> states = new Dictionary<EState, BaseState<EState>>();
    
    protected BaseState<EState> currentState;
    public EState state;

    protected bool isTransitioning = false;
    protected bool isInitialized = false;
    
    void Start()
    {
        currentState.EnterState();
    }

    public virtual void Update()
    {
        if (!isInitialized) return;
        EState nextStateKey = currentState.GetNextState();
        if (!isTransitioning && nextStateKey.Equals(currentState.StateKey))
        {
            currentState.UpdateState();
        }
        else
        {
            TransistionToState(nextStateKey);
        }
    }

    public void TransistionToState(EState nextState)
    {
        if (!isInitialized) return;
        
        isTransitioning = true;
        currentState.ExitState();
        currentState = states[nextState];
        state = nextState; // So we can see this in the editor
        currentState.EnterState();
        isTransitioning = false;
    }
}
