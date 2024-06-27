using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class BTComposite : BTNode
{
    protected List<BTNode> m_nodes = new List<BTNode>();
    protected List<BTNode>.Enumerator m_itr;
    protected BTNode m_currentNode;

    public void Init(BTContext context)
    {
        m_itr = m_nodes.GetEnumerator();
        pickNextNode(context);
    }

    public BTStatus Tick(BTContext context)
    {
        if (m_nodes.Count == 0)
        {
            throw new System.Exception("Trying to tick an empty composite node");
        }
        return tickCurrentNode(context);
    }

    protected BTStatus defaultTick(BTContext context, BTStatus goUntilStatus)
    {
        BTStatus status = m_currentNode.Tick(context);
        if (status == BTStatus.Running || status == goUntilStatus)  // if the currently ticked node is running or it's status is the end status (for sequential this is false, selector this is true)
        {
            return status;
        }
        else
        {
            if (pickNextNode(context))  // if this is not the last node
            {
                return BTStatus.Running;
            }
            else
            {
                return status;
            }
        }
    }

    protected abstract BTStatus tickCurrentNode(BTContext context);

    public void AddChild(BTNode node)
    {
        if (!m_nodes.Contains(node))
        {
            m_nodes.Add(node);
        }
        else
        {
            throw new System.Exception("Trying to add the same node to a composite twice");
        }
        // m_itr = m_nodes.GetEnumerator();     * IF I WANT TO ADD NODES AT RUNTIME, I HAVE TO ADD THIS LINE. CURRENTLY, I AM NOT ADDING NODES AT RUNTIME *
    }

    public void RemoveAllChildren()
    {
        m_nodes.Clear();
        m_currentNode = null;
        m_itr = m_nodes.GetEnumerator();
    }

    public void RemoveChild(BTNode node)
    {
        if (m_nodes.Contains(node))
        {
            m_nodes.Remove(node);
            // m_itr = m_nodes.GetEnumerator();     * IF I WANT TO ADD NODES AT RUNTIME, I HAVE TO ADD THIS LINE. CURRENTLY, I AM NOT ADDING NODES AT RUNTIME *
        }
        else
        {
            Debug.Log("Trying to remove a node that is not a child on a selector. Has it already been removed?");
        }
    }

    protected bool pickNextNode(BTContext context)
    {
        if (m_itr.MoveNext())
        {
            m_currentNode = m_itr.Current;
            m_currentNode.Init(context);
            return true;
        }
        else
        {
            m_currentNode = null;
            return false;
        }
    }
}

public class BTSequence : BTComposite
{
    protected override BTStatus tickCurrentNode(BTContext context)
    {
        return defaultTick(context, BTStatus.Failure);
    }
}

public class BTSelector : BTComposite
{
    protected override BTStatus tickCurrentNode(BTContext context)
    {
        return defaultTick(context, BTStatus.Success);
    }
}
