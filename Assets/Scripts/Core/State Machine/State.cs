using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public abstract class BaseState
{
    public virtual void Enter() { return; }
    public virtual void Update() { return; }
    public virtual void Exit() { return; }
    public virtual void PhysicsUpdate() { return; }
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
