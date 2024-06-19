using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class FrogPiece : Piece
{

    public FrogPiece()
    {
        m_ID = 1;
    }

    public override GameObject Spawn(Vector2 pos, string name = "")
    {
        m_SM = new StateMachine(FrogState.Idle);
        return base.Spawn(pos, "Frog");
    }

    public override void Action()
    {
        throw new System.NotImplementedException();
    }

    public override void Remove()
    {
        throw new System.NotImplementedException();
    }
}

public struct FrogState
{
    public static FrogIdle Idle = FrogIdle.Get();
    public static FrogJump Jump = FrogJump.Get();
}

public class FrogIdle : State<FrogIdle>
{
    public override void Enter(Piece piece)
    {
        piece.m_SM.Timer += 0.2f;
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
    public override void Enter(Piece piece)
    {
        BattleMaster.Instance.StartCoroutine(Jump(piece));
    }

    public override void PhysicsUpdate(Piece piece)
    {
        //piece.Teleport(Random.insideUnitCircle); 
    }

    private IEnumerator Jump(Piece piece)
    {
        yield return BattleMaster.Instance.StartCoroutine(piece.Move(piece.Position, piece.Position + (Vector3)Random.insideUnitCircle * 2, 0.75f));

        piece.m_SM.ToState(piece, FrogState.Idle);
    }
}
