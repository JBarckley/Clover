using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FrogState
{
    public static FrogIdle Idle = FrogIdle.Get();
    public static FrogJump Jump = FrogJump.Get();
}

public class FrogIdle : State<FrogIdle>
{
    TimerInstance IdleDelay;

    StateMachine sm = StateMachine.Get();

    public override void Enter()
    {
        IdleDelay = new TimerInstance(0.5f);
    }

    public override void Update()
    {
        if (!IdleDelay)
        {
            sm.ToState(FrogState.Jump);
        }
    }
}

public class FrogJump : State<FrogJump>
{
    public override void Update()
    {
        // do jump
    }
}
