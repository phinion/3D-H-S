using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }

    public void Initialize(PlayerState _startingState)
    {
        CurrentState = _startingState;
        Debug.Log(CurrentState);
        CurrentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        CurrentState.Exit();
        CurrentState = _newState;
        CurrentState.Enter();
        Debug.Log("change state: " + CurrentState);
    }
}
