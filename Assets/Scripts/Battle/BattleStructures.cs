using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

// board structure used in construction of the pre-battle phase board
public struct GameBoard
{
    public GameBoard(PieceLayout PlayerPieces)
    {
        m_Board = new Piece[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int k = 0; k < 8; k++)
            {
                m_Board[i, k] = new NonePiece();
            }
        }
        PopulateFromPlayer(PlayerPieces);
    }

    public void PopulateFromPlayer(PieceLayout PlayerPieces)
    {
        for (int i = 0; i < Mathf.Min(PlayerPieces.GetLength(0), 2); i++)
        {
            for (int k = 0; k < Mathf.Min(PlayerPieces.GetLength(1), 8); k++)
            {
                //Debug.Log(m_Board[i, k] + " " + PlayerPieces[i, k]);
                m_Board[i, k] = PlayerPieces[i, k];
            }
        }
    }

    public void Update()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int k = 0; k < 8; k++)
            {
                if (m_Board[i, k].m_SM != null)
                {
                    StateMachine sm = m_Board[i, k].m_SM;
                    sm.Current.Update(m_Board[i, k]);
                }
            }
        }
    }

    public Piece this[int x, int y]
    {
        get
        {
            return m_Board[x, y];
        }
        set
        {
            m_Board[x, y] = value;
        }
    }

    public float GetLength(int dim)
    {
        switch (dim)
        {
            case 0:
                return m_Board.GetLength(0);
            case 1:
                return m_Board.GetLength(1);
            case -1:
                return m_Board.Length;
            default:
                return -1;
        }
    }

    public void Print()
    {
        string board = "";
        for (int i = 7; i > -1; i--)
        {
            for (int k = 0; k < 8; k++)
            {
                board += (PieceName)m_Board[i, k].GetID() + " ";
            }
            board += "\n";
        }
        Debug.Log(board);
    }

    private Piece[,] m_Board;
}

public struct PieceLayout
{
    public PieceLayout(PieceName[,] Layout)
    {
        m_PieceLayout = new Piece[2, 8];
        PopulatePieceLayout(Layout);
    }

    public void PopulatePieceLayout(PieceName[,] Layout)
    {
        for (int i = 0; i < Mathf.Min(Layout.GetLength(0), 2); i++)
        {
            for (int k = 0; k < Mathf.Min(Layout.GetLength(1), 8); k++)
            {
                m_PieceLayout[i, k] = PieceFactory.CreatePiece(Layout[i, k]);
            }
        }
    }
    public float GetLength(int dim)
    {
        switch (dim)
        {
            case 0:
                return m_PieceLayout.GetLength(0);
            case 1:
                return m_PieceLayout.GetLength(1);
            case -1:
                return m_PieceLayout.Length;
            default:
                return -1;
        }
    }

    public Piece this[int x, int y]
    {
        get
        {
            return m_PieceLayout[x, y];
        }
        set
        {
            m_PieceLayout[x, y] = value;
        }
    }

    private Piece[,] m_PieceLayout;
}

// board used and dynamically updated with the flow of battle
public static class BattleBoard
{
    private static Dictionary<Piece, Vector3> Board = new Dictionary<Piece, Vector3>();

    public static void Create(GameBoard GameBoard)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int k = 0; k < 8; k++)
            {
                Piece piece = GameBoard[i, k];
                if (piece.Name != PieceName.None)
                {
                    Board.Add(GameBoard[i, k], GameBoard[i, k].Instance.transform.position);
                }
            }
        }
    }

    public static Piece Find(Piece piece)
    {
        return piece; // not yet implemented
    }

    public static void Move(Piece piece, Vector2 move)
    {
        Debug.Log("here2" + (piece.Instance.transform.position + (Vector3)move));
        if ((piece.Instance.transform.position + (Vector3)move).WithinBoardBoundary())
        {
            Debug.Log("here3");
            piece.Instance.transform.position += (Vector3)move;
            Board[piece] = piece.Instance.transform.position;
        }
    }
}

public struct Boundary
{
    public static float Top = 5.6f;
    public static float Bottom = -Top;
    public static float Right = -25f + Top;
    public static float Left = -25f + Bottom;
}

public static class VectorExtension
{
    public static bool WithinBoardBoundary(this Vector3 vec)
    {
        return vec.x < Boundary.Right && vec.x > Boundary.Left && vec.y > Boundary.Bottom && vec.y < Boundary.Top;
    }
}
