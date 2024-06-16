using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratePiece : Piece
{
    public CratePiece()
    {
        m_ID = 2;
    }

    public override GameObject Spawn(Vector2 pos, string name = "")
    {
        m_SM = new StateMachine(CrateState.Idle); // using a state machine here is unnecessary, but I like sticking with the convention for the few edge cases.
        return base.Spawn(pos, "Crate");
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

public struct CrateState
{
    public static CrateIdle Idle = new CrateIdle();
}

public class CrateIdle : State<CrateIdle>
{
    public override void Update(Piece piece)
    {
        // empty
    }
}
