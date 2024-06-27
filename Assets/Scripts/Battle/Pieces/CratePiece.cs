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

        m_SM = new StateMachine(this, CrateState.Idle); // using a state machine here is unnecessary, but I like sticking with the convention for the few edge cases.

        //BTEmptyLeaf Empty = new BTEmptyLeaf();
        //BTree CrateBehavior = new BTree(Empty, this);
        BTree CrateBehavior = new BTree("Assets/Resources/Pieces/Crate/Crate.xml", this);

        m_Behavior = CrateBehavior;
    }

    public override void Action()
    {
        throw new System.NotImplementedException();
    }

    public override void Remove()
    {
        throw new System.NotImplementedException();
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

public class BTEmptyLeaf : BTLeaf
{
    public override BTStatus Tick(BTContext context)
    {
        return BTStatus.Success;
    }
}
