using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

// board structure used in construction of the pre-battle phase board
public struct GameBoard
{
    public GameBoard(PieceLayout PlayerPieces, PieceLayout OpponentPieces)
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
        PopulateFromOpponent(OpponentPieces);
    }

    public void PopulateFromPlayer(PieceLayout PlayerPieces)
    {
        for (int i = 0; i < Mathf.Min(PlayerPieces.GetLength(0), 2); i++)
        {
            for (int k = 0; k < Mathf.Min(PlayerPieces.GetLength(1), 8); k++)
            {
                m_Board[i, k] = PlayerPieces[i, k];
            }
        }
    }

    public void PopulateFromOpponent(PieceLayout OpponentPieces)
    {
        for (int i = 0; i < Mathf.Min(OpponentPieces.GetLength(0), 2); i++)
        {
            for (int k = 0; k < Mathf.Min(OpponentPieces.GetLength(1), 8); k++)
            {
                m_Board[7 - i, k] = OpponentPieces[i, k];
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

public struct Boundary
{
    public static float Top = 5.6f;
    public static float Bottom = -Top + 0.5f;
    public static float Right = -25f + Top;
    public static float Left = -25f - Top;
}

public static class VectorExtension
{
    public static bool IsWithinBoardBoundary(this Vector3 vec)
    {
        return vec.x < Boundary.Right && vec.x > Boundary.Left && vec.y > Boundary.Bottom && vec.y < Boundary.Top;
    }

    public static Vector3 WithinBoardBoundary(this Vector3 vec)
    {
        return new Vector3(Mathf.Clamp(vec.x, Boundary.Left, Boundary.Right), Mathf.Clamp(vec.y, Boundary.Bottom, Boundary.Top), vec.z);
    }

    public static Vector3 BoardIntersection(this Vector3 vec, Vector3 direction)
    {
        float IntersectionX;
        float IntersectionY;
        float IntersectionLine;
        Vector3 Intersection;

        if (vec.x > 0)  // going towards top
        {
            IntersectionLine = Boundary.Top;
        }
        else
        {
            IntersectionLine = Boundary.Bottom;
        }

        IntersectionY = (IntersectionLine - vec.y) / direction.y;
        IntersectionX = (vec.x + (direction.x * IntersectionY)) / IntersectionLine;
        Intersection = new Vector3(IntersectionX, IntersectionY, 0f);

        if (Intersection.WithinBoardBoundary() == Intersection)
        {
            return Intersection;
        }
        else    // intersection is going towards a side
        {
            if (vec.y > 0)
            {
                IntersectionLine = Boundary.Right;
            }
            else
            {
                IntersectionLine = Boundary.Left;
            }

            IntersectionX = (IntersectionLine - vec.x) / direction.x;
            IntersectionY = (vec.y + (direction.y * IntersectionY)) / IntersectionLine;

            Intersection = new Vector3(IntersectionX, IntersectionY, 0f);
            return Intersection;
        }
        
    }
}
