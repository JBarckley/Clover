using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BTDecorator : BTNode
{
    protected BTNode m_currentNode;

    public virtual void Init(BTContext context)
    {
        m_currentNode.Init(context);
    }

    public abstract BTStatus Tick(BTContext context);

    public virtual void AddChild(BTNode node)
    {
        if (m_currentNode == null)
        {
            m_currentNode = node;
        }
        else
        {
            Debug.Log("Adding child to decorator already populated with a child, are you sure this is correct?");
            m_currentNode = node;
        }
    }

    public virtual void RemoveAllChildren()
    {
        m_currentNode = null;
    }

    public virtual void RemoveChild(BTNode node)
    {
        m_currentNode = null;
    }
}

public class BTInverter : BTDecorator
{
    public override BTStatus Tick(BTContext context)
    {
        return m_currentNode.Tick(context).Invert();
    }
}

public abstract class BTFixedDecorator : BTDecorator
{
    protected abstract BTStatus FixedStatus();

    public override BTStatus Tick(BTContext context)
    {
        return m_currentNode.Tick(context).Fixed(FixedStatus());
    }
}

public class BTSuccess : BTFixedDecorator
{
    protected override BTStatus FixedStatus()
    {
        return BTStatus.Success;
    }
}

public class BTFailure : BTFixedDecorator
{
    protected override BTStatus FixedStatus()
    {
        return BTStatus.Failure;
    }
}

public class BTRepeater : BTDecorator
{
    public override BTStatus Tick(BTContext context)
    {
        BTStatus status = m_currentNode.Tick(context);
        if (status != BTStatus.Running)
        {
            m_currentNode.Init(context);        //      As soon as the child node has a result, we reinitialize it and do it again. This is essential for behaviors that loop / repeat endlessly.
        }
        return status;
    }
}

public abstract class BTRepeaterUntil : BTDecorator
{
    protected abstract BTStatus RepeatUntil();

    public override BTStatus Tick(BTContext context)
    {
        BTStatus status = m_currentNode.Tick(context);
        while (status != RepeatUntil())
        {
            status = m_currentNode.Tick(context);   //      * CAREFUL NOW, POSSIBLE INFINITE LOOP *
        }
        return BTStatus.Success;
    }
}

public class BTRepeatUntilSuccess : BTRepeaterUntil
{
    protected override BTStatus RepeatUntil()
    {
        return BTStatus.Success;
    }
}

public class BTRepeatUntilFailure : BTRepeaterUntil
{
    protected override BTStatus RepeatUntil()
    {
        return BTStatus.Failure;
    }
}


/// <remarks>
/// Required Context:
///     "delay"
///     - float: length of delay in seconds
/// </remarks>
public abstract class BTDelay : BTDecorator
{
    protected float m_Delay;
    
    protected TimerInstance m_Timer = new TimerInstance();

    public override void Init(BTContext context)
    {
        base.Init(context);

        m_Delay = (float)context.GetVariable("delay");
    }
}

public class BTDelayBefore : BTDelay
{
    public override void Init(BTContext context)
    {
        base.Init(context);

        m_Timer += m_Delay;
    }

    public override BTStatus Tick(BTContext context)
    {
        if (!m_Timer)
        {
            return BTStatus.Running;
        }
        return m_currentNode.Tick(context);
    }
}

public class BTDelayAfter : BTDelay
{
    private bool leafDone;
    private bool timerStarted;
    private BTStatus status;

    public override void Init(BTContext context)
    {
        base.Init(context);

        leafDone = false;
        timerStarted = false;
        status = BTStatus.Running;
    }

    public override BTStatus Tick(BTContext context)
    {
        if (!leafDone)
        {
            status = m_currentNode.Tick(context);
        }
        if (status != BTStatus.Running && !m_Timer && !timerStarted)
        {
            leafDone = true;
            timerStarted = true;
            m_Timer += m_Delay;
        }
        else if (!m_Timer && timerStarted)
        {
            return status;
        }
        return BTStatus.Running;
    }
}

public class BTContextSetter : BTDecorator
{
    public delegate void NewContext(BTContext context);

    private NewContext _newContext;

    public void SetContext(NewContext newContext)
    {
        _newContext = newContext;
    }

    public override void Init(BTContext context)
    {
        _newContext?.Invoke(context);
        m_currentNode.Init(context);
    }

    public override BTStatus Tick(BTContext context)
    {
        return m_currentNode.Tick(context);
    }
}
