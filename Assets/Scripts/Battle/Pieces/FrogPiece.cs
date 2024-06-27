using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
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

        m_SM = new StateMachine(this, FrogState.Idle);

        BTRepeater root = new BTRepeater();
        BTContextSetter FrogIdleContext = new BTContextSetter();
        FrogIdleContext.SetContext(SetFrogIdleContext);
        BTDelayAfter FrogIdle = new BTDelayAfter();
        BTContextSetter FrogJumpContext = new BTContextSetter();
        FrogJumpContext.SetContext(SetFrogJumpContext);
        BTMoveLeaf FrogJump = new BTMoveLeaf();
        root.AddChild(FrogIdleContext);
        FrogIdleContext.AddChild(FrogIdle);
        FrogIdle.AddChild(FrogJumpContext);
        FrogJumpContext.AddChild(FrogJump);

        //m_Behavior = new BTree(root, this);

        m_Behavior = new BTree("Assets/Resources/Pieces/Frog/Frog.xml", this);
    }

    public override void Action()
    {
        throw new System.NotImplementedException();
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
        //return 0.125f + (Random.value / 10);
        context.SetVariable("delay", 0.125f + (Random.value / 10));
    }
}

public struct FrogState
{
    public static FrogIdle Idle = FrogIdle.Get();
    public static FrogJump Jump = FrogJump.Get();
}

public class FrogIdle : State<FrogIdle>
{
    public override void Enter(Piece piece)
    {
        piece.Timer += 0.125f + (Random.value / 10);
    }

    public override void Update(Piece piece)
    {
        if (!piece.Timer)
        {
            Debug.Log("to jump");
            piece.ToState(FrogState.Jump);
        }
    }
}

public class FrogJump : State<FrogJump>
{
    public override void Enter(Piece piece)
    {
        BattleMaster.Instance.StartCoroutine(Jump(piece));
    }

    public override void PhysicsUpdate(Piece piece)
    {
        //piece.Teleport(Random.insideUnitCircle);
    }

    private IEnumerator Jump(Piece piece)
    {
        yield return BattleMaster.Instance.StartCoroutine(piece.Move(piece.Position, piece.Position + (Vector3)Random.insideUnitCircle * 2, 0.75f));

        piece.ToState(FrogState.Idle);
    }
}

public class BTMoveLeaf : BTLeaf
{
    Piece piece;

    float elapsedTime;
    //static readonly float duration = 0.75f;
    float duration;

    Vector3 start;
    Vector3 end;

    public override void Init(BTContext context)
    {
        piece = (Piece)context.GetVariable("piece");

        start = piece.Position;
        //end = (start + (Vector3)Random.insideUnitCircle * 2).WithinBoardBoundary();
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
