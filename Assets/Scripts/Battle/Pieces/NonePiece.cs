using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonePiece : Piece
{
    public NonePiece()
    {
        m_ID = 0;
    }

    public override GameObject Spawn(Vector2 pos, string name = "")
    {
        m_Instance = base.Spawn(pos, "None");
        return m_Instance;
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
