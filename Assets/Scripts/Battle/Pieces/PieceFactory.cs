using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PieceFactory
{
    public static Piece CreatePiece(PieceName pieceName)
    {
        switch (pieceName)
        {
            case PieceName.None: return new NonePiece();
            case PieceName.Frog: return new FrogPiece();
            default: return new NonePiece();
        }
    }


}
