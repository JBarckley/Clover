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
        Vector3 firstMove = Random.onUnitSphere * 100;
        firstMove.z = 0;
        context.SetVariable("end", firstMove);
    }
}

public class BTFireballReflectLeaf : BTLeaf
{
    public override BTStatus Tick(BTContext context)
    {
        return BTStatus.Success;
    }
}
