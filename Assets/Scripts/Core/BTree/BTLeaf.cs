using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BTLeaf : BTNode
{
    public virtual void Init(BTContext context) { }

    public virtual BTStatus Tick(BTContext context) { return BTStatus.Failure; }

    public void AddChild(BTNode node)
    {
        throw new System.Exception("BTLeaf node cannot add children");
    }

    public void RemoveChild(BTNode node)
    {
        throw new System.Exception("BTLeaf node cannot remove children");
    }

    public void RemoveAllChildren(BTNode node)
    {
        throw new System.Exception("BTLeaf node cannot remove all childen");
    }
}
