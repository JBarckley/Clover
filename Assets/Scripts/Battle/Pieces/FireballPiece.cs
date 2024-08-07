using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FireballPiece : Piece
{
    public AIPath ap;

    public FireballPiece()
    {
        m_ID = 3;
    }

    public override void Spawn(Vector2 pos, string name = "")
    {
        base.Spawn(pos, "Fireball");

        //m_Behavior = new BTree("Assets/Resources/Pieces/Fireball/Fireball.xml", this);

        //dgo = Instance.AddComponent<DynamicGridObstacle>();
        //dgo.updateError = 0.05f;
        //dgo.checkTime = 0.1f;

        ap = Instance.AddComponent<AIPath>();
        ap.autoRepath.maximumInterval = 0.03f;
        ap.pickNextWaypointDist = 0.1f;
        ap.radius = 0.5f;
        ap.maxSpeed = 4f;
        ap.gravity = new Vector3(0, 0, 0);
        ap.updateRotation = false;
        ap.orientation = OrientationMode.YAxisForward;
        ap.whenCloseToDestination = CloseToDestinationMode.ContinueToExactDestination;
        ap.destination = new Vector3(Instance.transform.position.x, Boundary.Bottom + 0.5f, 0);
        ap.canMove = true;
        ap.enabled = false;
    }

    public override void Remove()
    {
        throw new System.NotImplementedException();
    }
}

public class BTFireballMoveLeaf : BTMoveLeaf
{
    private byte PerGridUpdate;

    private Vector3 direction = Vector3.zero;

    public override void Init(BTContext context)
    {
        if (direction == Vector3.zero)
        {
            FireballFirstMove(context);
        }
        else
        {
            FireballReflect(context);
        }

        base.Init(context);
    }

    public override BTStatus Tick(BTContext context)
    {
        BTStatus Tick = base.Tick(context);

        // animations, ect

        //List<Piece> test = BattleBoard.KNN.NearestNeighbors[piece].PlayerPieces;
        //piece.KNN(8, "player");

        return Tick;
    }

    private void FireballReflect(BTContext context)
    {
        if (end.y < Boundary.Top && end.y > Boundary.Bottom)
        {
            // do reflect over y = 0
            direction.x *= -1;
        }
        else
        {
            // do reflect over x = 0
            direction.y *= -1;
        }

        // add a little randomness to the direction (if moving in straight line as in y near 0, dont push over axis lines)
        float RandomnessX = Random.Range(0f, Mathf.Lerp(0f, 0.075f, Mathf.Abs(direction.x)));
        float RandomnessY = Random.Range(0f, Mathf.Lerp(0f, 0.075f, Mathf.Abs(direction.y)));

        direction.x += RandomnessX;
        direction.y += RandomnessY;

        end = end.BoardIntersection(direction);
        context.SetVariable("end", end);
    }

    private void FireballFirstMove(BTContext context)
    {
        piece = (Piece)context.GetVariable("piece");
        direction = Random.insideUnitCircle;

        while (Mathf.Abs(direction.x) >= 0.95f || Mathf.Abs(direction.y) >= 0.95f)  // make sure we're not completely in one direction
        {
            direction = Random.insideUnitCircle;
        }

        Vector3 endPosition = piece.Position.BoardIntersection(direction);
        context.SetVariable("end", endPosition);
        context.SetVariable("speed", 3f);
    }
}
