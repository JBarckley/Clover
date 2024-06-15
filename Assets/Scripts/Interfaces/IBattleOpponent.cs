using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public interface IBattleOpponent
{
    /// <summary>
    /// This function retuns a 2D array of length 2x8 which represents the position of pieces on the opponent's board.
    /// </summary>
    /// <returns></returns>
    public float[,] GetEnemyPieces();
}
