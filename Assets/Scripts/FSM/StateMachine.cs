using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    public IState<T> CurrentState { get; private set; }

    private T _controller;

    public StateMachine(T controller, IState<T> initialState)
    {
        if (controller == null)
        {
            Debug.LogError("StateMachine initialized with a null sender.");
            return;
        }

        _controller = controller;
        SetState(initialState);
    }

    public void SetState(IState<T> newState)
    {
        if (newState == null)
        {
            Debug.LogError("Attempting to set a null state.");            
            return;
        }

        if (CurrentState == newState) return;

        CurrentState?.Exit(_controller);        
        CurrentState = newState;
        CurrentState.Enter(_controller);
        
        Debug.Log($"State changed to {CurrentState.GetName()}.");        
    }

    public void ChangeState(IState<T> newState)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit(_controller);
        }

        CurrentState = newState;

        if (CurrentState != null)
        {
            CurrentState.Enter(_controller);
        }
    }

    public void Update()
    {
        CurrentState?.Update(_controller);
    }

    public void FixedUpdate()
    {
        CurrentState?.FixedUpdate(_controller);
    }

}