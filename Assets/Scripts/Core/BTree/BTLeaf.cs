using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public abstract class BTLeaf : BTNode
{
    public virtual void Init(BTContext context) { }

    public virtual BTStatus Tick(BTContext context) { Debug.Log("Ticking an abstract leaf??");  return BTStatus.Success; }

    public void AddChild(BTNode node)
    {
        throw new System.Exception("BTLeaf node cannot add children");
    }

    public void RemoveChild(BTNode node)
    {
        throw new System.Exception("BTLeaf node cannot remove children");
    }

    public void RemoveAllChildren()
    {
        throw new System.Exception("BTLeaf node cannot remove all childen");
    }
}

public class BTEmptyLeaf : BTLeaf
{
    public override BTStatus Tick(BTContext context)
    {
        return BTStatus.Success;
    }
}

/// <remarks>
/// Required Context:
///     "end:"
///     - Vector3: final position of movement.
///     "duration"
///     - float: duration of the movement.
/// </remarks>
public class BTMoveLeaf : BTLeaf
{
    Piece piece;

    float elapsedTime;
    float duration;

    Vector3 start;
    Vector3 end;

    public override void Init(BTContext context)
    {
        piece = (Piece)context.GetVariable("piece");

        start = piece.Position;
        end = (Vector3)context.GetVariable("end");
        duration = (float)context.GetVariable("duration");
        elapsedTime = 0f;
    }

    public override BTStatus Tick(BTContext context)
    {
        if (elapsedTime < duration)
        {
            piece.Instance.transform.position = Vector3.Lerp(start, end, elapsedTime / duration);
            piece.Position = piece.Instance.transform.position;
            elapsedTime += Time.deltaTime;
            return BTStatus.Running;
        }

        piece.Instance.transform.position = end;
        piece.Position = end;

        return BTStatus.Success;
    }
}

public class BTIsOnBoardLeaf : BTLeaf
{
    Piece piece;

    public override void Init(BTContext context)
    {
        base.Init(context);

        piece = (Piece)context.GetVariable("piece");
    }
    public override BTStatus Tick(BTContext context)
    {
        if (piece.Position.IsWithinBoardBoundary())
        {
            return BTStatus.Success;
        }
        else
        {
            return BTStatus.Failure;
        }
    }
}
