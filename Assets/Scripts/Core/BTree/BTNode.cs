using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BTNode
{
    public void Init(BTContext context);

    public BTStatus Tick(BTContext context);

    public void AddChild(BTNode node);

    public void RemoveChild(BTNode node);

    public void RemoveAllChildren();

}
