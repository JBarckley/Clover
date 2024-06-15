using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class StateMachine
{
    public BaseState Current;
    public TimerInstance Timer = new TimerInstance();

    public StateMachine(BaseState initState)
    {
        Debug.Log("new SM");
        Current = initState;
    }

    public void ToState(Piece piece, BaseState nextState)
    {
        Current.Exit(piece);
        Current = nextState;
        nextState.Enter(piece);
    }
}
