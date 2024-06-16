using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleMaster : MonoBehaviour
{

    private Camera cam;
    private GameObject boardInstance;

    public GameBoard Board;

    void Update()
    {
        if (GamePhase.Current == Phase.Battle)
        {
            BattleControl.Update(Board);
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

    private static PieceLayout MinimalPlayer = new PieceLayout(new PieceName[,] {{ PieceName.None, PieceName.None, PieceName.None, PieceName.Frog, PieceName.Frog, PieceName.None, PieceName.None, PieceName.None},
                                                                                 { PieceName.None, PieceName.None, PieceName.None, PieceName.None, PieceName.None, PieceName.None, PieceName.None, PieceName.None}});
    private static PieceLayout MinimalOpponent = new PieceLayout(new PieceName[,] {{ PieceName.None, PieceName.None, PieceName.None, PieceName.None, PieceName.Crate, PieceName.None, PieceName.None, PieceName.None},
                                                                                   { PieceName.None, PieceName.None, PieceName.None, PieceName.None, PieceName.None, PieceName.None, PieceName.None, PieceName.None}});
    private static PieceLayout MaximalPlayer = new PieceLayout(new PieceName[,] {{ PieceName.Frog, PieceName.Frog, PieceName.Frog, PieceName.Frog, PieceName.Frog, PieceName.Frog, PieceName.Frog, PieceName.Frog},
                                                                                 { PieceName.None, PieceName.Frog, PieceName.None, PieceName.Frog, PieceName.None, PieceName.Frog, PieceName.None, PieceName.Frog}});
    private static PieceLayout MaximalOpponent = new PieceLayout(new PieceName[,] {{ PieceName.Crate, PieceName.Crate, PieceName.Crate, PieceName.Crate, PieceName.Crate, PieceName.Crate, PieceName.Crate, PieceName.Crate},
                                                                                   { PieceName.None, PieceName.None, PieceName.Crate, PieceName.Crate, PieceName.Crate, PieceName.Crate, PieceName.None, PieceName.None}});

    public void SetupBattle()
    {
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        boardInstance = (GameObject) Instantiate(Resources.Load("World/Board"), transform.position, Quaternion.identity);
        PieceLayout PlayerPieces = MaximalPlayer;
        PieceLayout OpponentPieces = MaximalOpponent;
        Board = new GameBoard(PlayerPieces, OpponentPieces);
        BattleControl.SpawnPieces(Board, boardInstance.transform.position);
        Board.Print();
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
