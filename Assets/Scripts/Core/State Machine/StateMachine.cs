using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class StateMachine
{
    public BaseState Current;
    public Piece Master;

    public StateMachine(Piece piece, BaseState initState)
    {
        Debug.Log("new SM");
        Current = initState;
        Master = piece;
    }

    public void ToState(BaseState nextState)
    {
        Current.Exit(Master);
        Current = nextState;
        nextState.Enter(Master);
    }
}
