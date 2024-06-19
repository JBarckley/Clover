using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class Master : MonoSingleton<Master>
{
    private Player m_Player;
    private BattleMaster m_BattleMaster;
    private Camera m_Camera;
    private InputHandler m_InputHandler;

    private void Start()
    {
        gameObject.AddComponent<InputHandler>(BattleMaster.StartBattle);

        m_Player = Player.Instance;
        m_BattleMaster = BattleMaster.Instance;
        m_Camera = GameCamera.Cam;
        m_InputHandler = InputHandler.Instance;

        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        m_Player.Move(InputHandler.moveVector);
    }

    public void Battle()
    {
        if (GamePhase.Current == Phase.World)
        {
            GamePhase.Current = Phase.PreBattle;
            BattleMaster.SetupBattle();
        }
        else
        {
            throw new System.AccessViolationException("Tried to access battle from somewhere not the world!");
        }
    }
}

public static class GamePhase
{
    public static Phase Current = Phase.World;
}

public enum Phase
{
    World,
    PreBattle,
    Battle,
    PostBattle
}
