using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
