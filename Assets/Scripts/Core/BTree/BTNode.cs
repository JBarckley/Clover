using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BTNode
{
    public void Init(BTContext context);

    BTStatus Tick(BTContext context);

    void AddChild(BTNode node);

    void RemoveChild(BTNode node);

    void RemoveAllChildren(BTNode node);

}
