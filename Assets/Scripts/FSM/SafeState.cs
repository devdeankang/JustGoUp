using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
IState<T>를 감싸, 각 메소드 호출 전에 안전성 검사를 수행하는 클래스
**/

public class SafeState<T> : IState<T> where T : class
{
    private IState<T> _wrappedState;

    public SafeState(IState<T> wrappedState)
    {
        _wrappedState = wrappedState ?? throw new ArgumentNullException(nameof(wrappedState), "Wrapped state cannot be null.");
    }

    public void Enter(T player)
    {
        if (player == null)
        {
            Debug.LogError("Player is null on Enter");
            return;
        }
        _wrappedState.Enter(player);
    }

    public void Update(T player)
    {
        if (player == null)
        {
            Debug.LogError("Player is null on Update");
            return;
        }
        _wrappedState.Update(player);
    }

    public void FixedUpdate(T player)
    {
        if (player == null)
        {
            Debug.LogError("Player is null on FixedUpdate");
            return;
        }
        _wrappedState.FixedUpdate(player);
    }

    public void Exit(T player)
    {
        if (player == null)
        {
            Debug.LogError("Player is null on Exit");
            return;
        }
        _wrappedState.Exit(player);
    }

    public string GetName()
    {
        return _wrappedState.GetName();
    }
}
