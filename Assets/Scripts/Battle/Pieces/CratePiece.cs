using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratePiece : Piece
{
    public CratePiece()
    {
        m_ID = 2;
    }

    public override void Spawn(Vector2 pos, string name = "")
    {
        base.Spawn(pos, "Crate");

        m_Behavior = new BTree("Assets/Resources/Pieces/Crate/Crate.xml", this);
    }

    public override void Remove()
    {
        throw new System.NotImplementedException();
    }
}
