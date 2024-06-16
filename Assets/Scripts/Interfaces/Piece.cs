using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class Piece
{
    protected Piece()
    {
        m_ID = 255;
    }

    public int GetID()
    {
        return m_ID;
    }

    public virtual GameObject Spawn(Vector2 pos, string name = "")
    {
        if (name == "None") return null; // None pieces are stored as a default circle sprite for debug purposes, but this line removes them completely.
        return (GameObject)Object.Instantiate(Resources.Load("Pieces/" + name, typeof(GameObject)), pos, Quaternion.identity);
    }

    public abstract void Action();

    public abstract void Remove();

    public override string ToString()
    {
        return ((PieceName)m_ID).ToString();
    }

    protected byte m_ID;

    public GameObject Instance { get; protected set; }
    public Vector3 Position; // set position in the setter of Instance

    public StateMachine m_SM = null;

    public PieceName Name 
    { 
        get
        {
            return (PieceName)m_ID;
        }
    }
}

public enum PieceName
{
    None,
    Frog,
    Crate
}
