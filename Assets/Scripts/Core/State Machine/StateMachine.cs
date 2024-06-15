using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class StateMachine
{
    private static StateMachine _instance = new StateMachine();
    public static StateMachine Instance { get { return _instance; } }
    public static StateMachine Get() 
    { 
        return Instance; 
    }

    public BaseState State;

    public void Init(BaseState initState)
    {
        State = initState;
    }

    public void ToState(BaseState nextState)
    {
        State.Exit();
        State = nextState;
        nextState.Enter();
    }
}
