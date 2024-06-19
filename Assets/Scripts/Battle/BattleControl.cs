using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  Spawn the "grid" and the pieces doing battle. Perform the necessary setup before handing the battle itself over to a different class.
///  
///  Potentially, setup the NPC interactions during the battle.
/// </summary>

public static class BattleControl
{
    private static float scale = 0.7f;

    public static void SpawnPieces(GameBoard board, Vector3 boardPosition)
    {
        Vector2 basePos = new Vector2(boardPosition.x - ((8 - 1) * scale), boardPosition.y - ((8 - 1) * scale));
        for (int i = 0; i < 8; i++)
        {
            for (int k = 0; k < 8; k++)
            {
                Vector2 pos = basePos + new Vector2(k * 2 * scale, i * 2 * scale);
                board[i, k].Spawn(pos);
            }
        }
    }

    public static void Update()
    {
        BattleBoard.Update();
    }

    public static void PhysicsUpdate()
    {
        BattleBoard.PhysicsUpdate();
    }
}
