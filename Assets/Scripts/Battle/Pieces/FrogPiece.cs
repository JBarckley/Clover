using System.Collections;
using System.Collections.Generic;
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
