using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonePiece : Piece
{
    public NonePiece()
    {
        m_ID = 0;
    }

    public override void Spawn(Vector2 pos, string name = "")
    {
        base.Spawn(pos, "None");
    }

    public override void Remove()
    {
        throw new System.NotImplementedException();
    }
}
