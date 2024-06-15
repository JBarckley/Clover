using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Master : MonoBehaviour
{
    private Player m_Player;
    private BattleMaster m_BattleMaster;
    private Camera m_Camera;
    private InputHandler m_InputHandler;

    private void Awake()
    {
        m_Player = FindObjectOfType<Player>();
        m_BattleMaster = FindObjectOfType<BattleMaster>();
        m_Camera = FindObjectOfType<Camera>();
        m_InputHandler = gameObject.AddComponent<InputHandler>(m_BattleMaster.StartBattle);
    }

    private void Update()
    {
        m_Player.Move(m_InputHandler.moveVector);
        //Debug.Log(m_InputHandler.enterBattleButton);
    }

    public void Battle()
    {
        if (GamePhase.Current == Phase.World)
        {
            GamePhase.Current = Phase.PreBattle;
            m_BattleMaster.SetupBattle();
        }
        else
        {
            throw new System.AccessViolationException("Tried to access battle from somewhere not the world!");
        }
    }
}

public struct GamePhase
{
    private static GamePhase _instance = new GamePhase();
    public static GamePhase Instance { get { return _instance; } }
    public static Phase Current = Phase.World;
}

public enum Phase
{
    World,
    PreBattle,
    Battle,
    PostBattle
}
