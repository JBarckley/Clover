using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct FrogState
{
    public static FrogIdle Idle = FrogIdle.Get();
    public static FrogJump Jump = FrogJump.Get();
}

public class FrogIdle : State<FrogIdle>
{
    public override void Enter(Piece piece)
    {
        piece.m_SM.Timer += 1.5f;
    }

    public override void Update(Piece piece)
    {
        StateMachine sm = piece.m_SM;
        if (!sm.Timer)
        {
            Debug.Log("to jump");
            sm.ToState(piece, FrogState.Jump);
        }
    }
}

public class FrogJump : State<FrogJump>
{
    public override void Update(Piece piece)
    {
        BattleBoard.Move(piece, Random.insideUnitCircle);
        piece.m_SM.ToState(piece, FrogState.Idle);
    }
}
