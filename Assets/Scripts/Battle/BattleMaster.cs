using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleMaster : MonoSingleton<BattleMaster>
{
    private static GameObject boardInstance;
    public GameBoard Board;

    private AstarPath Pathfinder;

    private Camera cam;

    private void Start()
    {
        Pathfinder = AstarPath.active;
    }

    void Update()
    {
        if (GamePhase.Current == Phase.Battle)
        {
            //BattleBoard.KNN.FindKNN(8);
            BattleControl.Update();
        }
    }

    void FixedUpdate()
    {
        if (GamePhase.Current == Phase.Battle)
        {
            BattleControl.PhysicsUpdate();
        }
    }

    // The camera's projection size is:
    // height = 2 * orthographic size
    // width = height * aspect ratio
    private void OnDrawGizmos()
    {
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        float height = camHeight / 2f;
        float width = camWidth / 2f;

        Vector3 topLeft = new Vector3(-width, height, 0) + transform.position;
        Vector3 topRight = new Vector3(width, height, 0) + transform.position;
        Vector3 bottomRight = new Vector3(width, -height, 0) + transform.position;
        Vector3 bottomLeft = new Vector3(-width, -height, 0) + transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }

    private void OnValidate()
    {
        cam = FindObjectOfType<Camera>();
    }

    private static PieceLayout None = BattleControl.CreateEmptyLayout();
    private static PieceLayout MinimalPlayer = BattleControl.CreateDoubleLayout(PieceName.Frog);
    private static PieceLayout MinimalOpponent = BattleControl.CreateSingleLayout(PieceName.Crate);
    private static PieceLayout MaximalPlayer = BattleControl.CreateFullLayout(PieceName.Frog);
    private static PieceLayout MaximalPlayer2 = BattleControl.CreateFullLayout(PieceName.Frog);
    private static PieceLayout MaximalOpponent = BattleControl.CreateFullLayout(PieceName.Crate);
    private static PieceLayout OneFireball = BattleControl.CreateSingleLayout(PieceName.Fireball);
    private static PieceLayout FullFireball = BattleControl.CreateFullLayout(PieceName.Fireball);

    public void SetupBattle()
    {
        GameCamera.Teleport(GetPosition2());
        boardInstance = (GameObject) Instantiate(Resources.Load("World/Board"), GetPosition3(), Quaternion.identity);
        PieceLayout PlayerPieces = MaximalPlayer;
        PieceLayout OpponentPieces = FullFireball;
        Board = new GameBoard(PlayerPieces, OpponentPieces);
        BattleControl.SpawnPieces(Board, boardInstance.transform.position);
        Board.Print();

        GridGraph gg = Pathfinder.data.AddGraph(typeof(GridGraph)) as GridGraph;
        gg.is2D = true;
        gg.collision.use2D = true;
        gg.center = boardInstance.transform.position;
        gg.SetDimensions(33, 33, 0.337f);
        Pathfinder.Scan();

    }

    public void StartBattle(InputAction.CallbackContext context)
    {
        if (GamePhase.Current == Phase.PreBattle)
        {
            Debug.Log("Starting Battle!");

            GamePhase.Current = Phase.Battle;
            BattleBoard.Create(Board);
        }
    }
}
