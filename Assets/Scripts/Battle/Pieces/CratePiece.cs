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
        Instance = base.Spawn(pos, name);
        m_SM = new StateMachine(FrogState.Jump);
        return Instance;
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
