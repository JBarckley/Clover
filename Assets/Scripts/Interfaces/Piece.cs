using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        Instance = (GameObject)Object.Instantiate(Resources.Load("Pieces/" + name, typeof(GameObject)), pos, Quaternion.identity);
        Position = Instance.transform.position;
        return Instance;
    }

    public void Update()
    {
        m_SM.Current.Update(this);
    }

    public void PhysicsUpdate()
    {
        m_SM.Current.PhysicsUpdate(this);
    }

    public virtual void Teleport(Vector3 displacement)
    {
        if ((Instance.transform.position + displacement).IsWithinBoardBoundary())
        {
            //Debug.Log("here3");
            Instance.transform.position += displacement;
            Position = Instance.transform.position;
        }
    }

    public virtual IEnumerator Move(Vector3 start, Vector3 end, float duration)
    {
        float elapsedTime = 0;
        end = end.WithinBoardBoundary();

        while (elapsedTime < duration)
        {
            Instance.transform.position = Vector3.Lerp(start, end, elapsedTime / duration);
            Position = Instance.transform.position;
            elapsedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        Instance.transform.position = end;
    }

    public abstract void Action();

    public abstract void Remove();



    public override string ToString()
    {
        return ((PieceName)m_ID).ToString();
    }

    public static implicit operator Vector3(Piece p) => p.Position;

    protected byte m_ID;

    public GameObject Instance { get; protected set; }
    public Vector3 Position; // set position in the setter of Instance

    public StateMachine m_SM = null;

    // static battleboard such that all pieces reference the same memory!!!!

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
