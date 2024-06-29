using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class FrogPiece : Piece
{
    public FrogPiece()
    {
        m_ID = 1;
    }

    public override void Spawn(Vector2 pos, string name = "")
    {
        base.Spawn(pos, "Frog");

        m_Behavior = new BTree("Assets/Resources/Pieces/Frog/Frog.xml", this);
    }

    public override void Remove()
    {
        throw new System.NotImplementedException();
    }

    public void SetFrogJumpContext(BTContext context)
    {
        context.SetVariable("end", (Position + (Vector3)Random.insideUnitCircle * 2).WithinBoardBoundary());
        context.SetVariable("duration", Random.value);
    }

    public void SetFrogIdleContext(BTContext context)
    {
        context.SetVariable("delay", 0.125f + (Random.value / 10));
    }
}
