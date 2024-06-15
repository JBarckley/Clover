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
        Instance = base.Spawn(pos, "Frog");
        m_SM = new StateMachine(FrogState.Idle);
        return Instance;
    }

    public override void Action()
    {
        
    }

    public override void Remove()
    {
        throw new System.NotImplementedException();
    }
}
