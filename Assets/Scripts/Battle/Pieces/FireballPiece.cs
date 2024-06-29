using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballPiece : Piece
{
    public FireballPiece()
    {
        m_ID = 2;
    }

    public override void Spawn(Vector2 pos, string name = "")
    {
        base.Spawn(pos, "Fireball");

        m_Behavior = new BTree("Assets/Resources/Pieces/Fireball/Fireball.xml", this);
    }

    public override void Remove()
    {
        throw new System.NotImplementedException();
    }

    public void SetFireballFirstMoveContext(BTContext context)
    {
        Vector3 firstMove = Random.onUnitSphere;
        Vector3 direction;

        while (firstMove.z >= 0.8 || firstMove.y >= 0.85f || firstMove.x >= 0.85f)  // make sure we're not completely in one direction
        {
            firstMove = Random.onUnitSphere;
        }
        direction = (firstMove - Position).normalized;

        firstMove = Position.BoardIntersection(direction);
        context.SetVariable("end", firstMove);
        context.SetVariable("direction", direction);
    }
}

public class BTFireballMoveLeaf : BTMoveLeaf
{
    public override BTStatus Tick(BTContext context)
    {
        BTStatus Tick = base.Tick(context);
        // do animation
        return Tick;
    }
}

public class BTFireballReflectLeaf : BTLeaf
{
    Vector3 endPosition;
    Vector3 direction;

    public override BTStatus Tick(BTContext context)
    {
        endPosition = (Vector3)context.GetVariable("end");
        direction = (Vector3)context.GetVariable("direction");

        if (endPosition.y < Boundary.Top && endPosition.y > Boundary.Bottom)
        {
            // do reflect over y = 0
            direction.x *= -1;
        }
        else
        {
            // do reflect over x = 0
            direction.y *= -1;
        }

        endPosition = endPosition.BoardIntersection(direction);
        context.SetVariable("end", endPosition);

        return BTStatus.Success;
    }
}
