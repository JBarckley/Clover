using Pathfinding;
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
    public AIPath ap;

    public FrogPiece()
    {
        m_ID = 1;
    }

    public override void Spawn(Vector2 pos, string name = "")
    {
        base.Spawn(pos, "Frog");

        //m_Behavior = new BTree("Assets/Resources/Pieces/Frog/Frog.xml", this);

        dgo = Instance.AddComponent<DynamicGridObstacle>();
        dgo.updateError = 0.05f;
        dgo.checkTime = 0.1f;

        ap = Instance.AddComponent<AIPath>();
        ap.autoRepath.maximumInterval = 0.03f;
        ap.pickNextWaypointDist = 0.1f;
        ap.radius = 1.25f;
        ap.maxSpeed = 4f;
        ap.gravity = new Vector3(0, 0, 0);
        ap.updateRotation = false;
        ap.orientation = OrientationMode.YAxisForward;
        ap.whenCloseToDestination = CloseToDestinationMode.ContinueToExactDestination;
        ap.destination = new Vector3(Instance.transform.position.x, Boundary.Top, 0);
        ap.canMove = true;
        ap.enabled = false;
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

        Piece nearestEnemy = piece.KNN(1, "enemy")?[0];
        if (nearestEnemy != null)
        {
            float randomMove = Random.Range(2f, 4f);
            Vector3 endPosition = Vector3.Lerp(piece.Position, nearestEnemy.Position, Mathf.Min(randomMove / (nearestEnemy.Position - piece.Position).magnitude, 1f));
            context.SetVariable("end", endPosition);
        }
        else
        {
            context.SetVariable("end", (piece.Position + (Vector3)Random.insideUnitCircle * 2).WithinBoardBoundary());
        }

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
