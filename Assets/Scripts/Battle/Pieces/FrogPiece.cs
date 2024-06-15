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
        m_Instance = base.Spawn(pos, "Frog");
        return m_Instance;
    }

    public override void Action()
    {
        TimerInstance MoveDelay = new TimerInstance(3.5f);
        if (!MoveDelay)
        {
            // move
        }
    }

    public override void Remove()
    {
        throw new System.NotImplementedException();
    }
}
