using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoSingleton<Player>
{
    private PlayerController m_PlayerController;

    private void Start()
    {
        m_PlayerController = gameObject.AddComponent<PlayerController>();
    }

    public void Move(Vector3 dir)
    {
        m_PlayerController.Move(dir);
    }
}

