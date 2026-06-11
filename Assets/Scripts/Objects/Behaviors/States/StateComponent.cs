using System;
using UnityEngine;

public class StateComponent : MonoBehaviour, IObjectBehavior
{
    private bool isActive;
    private bool currentState;

    public Action OnTurnOn;
    public Action OnTurnOff;

    public void Configure(LevelObjectParameters data)
    {
        isActive = data.hasStates;
        currentState = data.initialState;
    }

    public void ChangeState(bool newState)
    {
        if ( !isActive || currentState == newState) return;
        
        currentState = newState;
        if (currentState)
        {
            OnTurnOn?.Invoke();
        }
        else
        {
            OnTurnOff?.Invoke();
        }
    }
}
