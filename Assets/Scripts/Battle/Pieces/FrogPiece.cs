using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Timeline.TimelinePlaybackControls;

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
        context.SetVariable("speed", 3.5f);
    }

    public void SetFrogIdleContext(BTContext context)
    {
        context.SetVariable("delay", 0.125f + (Random.value / 10));
    }
}

public class BTFrogMoveLeaf : BTMoveLeaf
{
    public override void Init(BTContext context)
    {
        piece = (Piece)context.GetVariable("piece");

        context.SetVariable("end", (piece.Position + (Vector3)Random.insideUnitCircle * 2).WithinBoardBoundary());
        context.SetVariable("speed", 3.5f);

        base.Init(context);
    }
}

public class BTFrogDelayAfter : BTDelayAfter
{
    public override void Init(BTContext context)
    {
        context.SetVariable("delay", 0.125f + (Random.value / 10));

        base.Init(context);
    }
}
