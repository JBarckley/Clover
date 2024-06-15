using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    private PlayerController m_PlayerController;

    void Awake()
    {
        m_PlayerController = gameObject.AddComponent<PlayerController>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Move(Vector3 dir)
    {
        m_PlayerController.Move(dir);
    }
}


public static class Extension
{
    public static T AddComponent<T>(this Object obj, string test) where T : PlayerController
    {
        T rtn = obj.AddComponent<T>();
        rtn.name = test;
        return rtn;
    }
}

