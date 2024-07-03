using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class Piece
{
    protected byte m_ID;

    public GameObject Instance { get; protected set; }
    public Vector3 Position;

    public BTree m_Behavior = null;
    public TimerInstance Timer = new TimerInstance();

    public PieceName Name
    {
        get
        {
            return (PieceName)m_ID;
        }
    }

    protected Piece()
    {
        m_ID = 255;
    }

    public int GetID()
    {
        return m_ID;
    }

    public virtual void Spawn(Vector2 pos, string name = "")
    {
        if (name == "None") return; // None pieces are stored as a default circle sprite for debug purposes, but this line removes them completely.
        Instance = (GameObject)Object.Instantiate(Resources.Load("Pieces/" + name + "/" + name, typeof(GameObject)), pos, Quaternion.identity);
        Position = Instance.transform.position;
    }

    public void Update()
    {
        m_Behavior.Tick();
    }

    public void PhysicsUpdate()
    {
        //m_SM.Current.PhysicsUpdate(this);
    }

    public abstract void Remove();

    public override string ToString()
    {
        return ((PieceName)m_ID).ToString();
    }

    public List<Piece> KNN(int k, string perspective)
    {
        return BattleBoard.K_Nearest(this, k, perspective);
    }

    public static implicit operator Vector3(Piece p) => p.Position;
}

public enum PieceName
{
    None,
    Frog,
    Crate,
    Fireball
}

public class PieceCompare : IComparer<Piece>
{
    public Piece reference;

    public PieceCompare(Piece referencePoint)
    {
        reference = referencePoint;
    }

    public int Compare(Piece left, Piece right)
    {
        float a = (left.Position - reference.Position).magnitude;
        float b = (right.Position - reference.Position).magnitude;

        return a.CompareTo(b);
    }
}
