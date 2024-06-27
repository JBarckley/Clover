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

    public void ToState(BaseState nextState)
    {
        m_SM.ToState(nextState);
    }

    public virtual void Spawn(Vector2 pos, string name = "")
    {
        if (name == "None") return; // None pieces are stored as a default circle sprite for debug purposes, but this line removes them completely.
        Instance = (GameObject)Object.Instantiate(Resources.Load("Pieces/" + name + "/" + name, typeof(GameObject)), pos, Quaternion.identity);
        Position = Instance.transform.position;
    }

    public void Update()
    {
        //m_SM.Current.Update(this);
        m_Behavior.Tick();
    }

    public void PhysicsUpdate()
    {
        //m_SM.Current.PhysicsUpdate(this);
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

    private readonly WaitForFixedUpdate waitFixedUpdate = new WaitForFixedUpdate();
    delegate void changeState();

    public virtual IEnumerator Move(Vector3 start, Vector3 end, float duration)
    {
        float elapsedTime = 0;
        end = end.WithinBoardBoundary();

        while (elapsedTime < duration)
        {
            Instance.transform.position = Vector3.Lerp(start, end, elapsedTime / duration);
            Position = Instance.transform.position;
            elapsedTime += Time.deltaTime;
            yield return waitFixedUpdate;
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
    public Vector3 Position;

    public StateMachine m_SM = null;
    public BTree m_Behavior = null;
    public TimerInstance Timer = new TimerInstance();

    // static battleboard such that all pieces reference the same memory!!!! -- done

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
