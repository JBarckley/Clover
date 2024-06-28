using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public abstract class BaseState
{
    public virtual void Enter(Piece piece) { return; }
    public virtual void Update(Piece piece) { return; }
    public virtual void Exit(Piece piece) { return; }
    public virtual void PhysicsUpdate(Piece piece) { return; }
}

// https://stackoverflow.com/questions/16745629/how-to-abstract-a-singleton-class
public abstract class State<T> : BaseState where T : State<T>, new()
{
    private static readonly Lazy<T> _instance =
        new(() => (Activator.CreateInstance(typeof(T), true) as T)!);
    public static T Instance => _instance.Value;

    public static T Get()
    {
        return Instance;
    }
}

/*
public struct FrogState
{
    public static FrogIdle Idle = FrogIdle.Get();
    public static FrogJump Jump = FrogJump.Get();
}

public class FrogIdle : State<FrogIdle>
{
    public override void Enter(Piece piece)
    {
        piece.Timer += 0.125f + (UnityEngine.Random.value / 10);
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

public struct CrateState
{
    public static CrateIdle Idle = new CrateIdle();
}

public class CrateIdle : State<CrateIdle>
{
    public override void Update(Piece piece)
    {
        // empty
    }
}
*/
