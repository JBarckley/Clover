using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Resolvers;
using Unity.VisualScripting;
using UnityEngine;

// board dynamically updated with the flow of battle
public static class BattleBoard
{
    private static List<Piece> Board = new List<Piece>();

    public static void Create(GameBoard GameBoard)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int k = 0; k < 8; k++)
            {
                Piece piece = GameBoard[i, k];
                if (piece.Name != PieceName.None)
                {
                    Board.Add(piece);
                }
            }
        }
    }

    public static Piece Find(Piece piece)
    {
        return piece; // not yet implemented
    }

    public static void Teleport(Piece piece, Vector2 move)
    {
        //Debug.Log("here2" + (piece.Instance.transform.position + (Vector3)move));
        if ((piece.Instance.transform.position + (Vector3)move).WithinBoardBoundary())
        {
            //Debug.Log("here3");
            piece.Instance.transform.position += (Vector3)move;
            piece.Position = piece.Instance.transform.position;
        }
    }

    /*
     *      Accidentally deleted my explanation, but tldr is I spent a hour-ish researching k-d trees, quadtrees, octotrees,
     *      R-trees pretty much any spacially ordered trees trying to figure out the most efficient data structure but I
     *      realized the size of my data set meant that brute force would be a very competitive strategy for nearest neighbor search.
     *      
     *      With a point quadtree, I could get O(nlogn) which is significantly faster than a O(n^2) brute force approach of course, but, like,
     *      I will likely have a maximum of 40 units this is called on, which means the performance difference is < 1ms per frame, something
     *      I can live with.
     *      For instance, a brute force approach with 24 pieces searching for k = 8 nearest neighbors took ~0.5ms per frame.
     *      
     *      A brute force implementation is significantly less time consuming to implement and not having to use a package allows for much
     *      more convinient specialization of the structure.
     *      
     */

    public static class BoardUtility
    {
        public static Dictionary<Piece, List<Piece>> NearestNeighbors = new Dictionary<Piece, List<Piece>>();

        public static bool AugmentKNN()
        {
            return true;
        }

        public static void FindKNN(int k)
        {
            NearestNeighbors.Clear();
            List<Piece> nn = new List<Piece>();
            if (k > Board.Count + 1) throw new IndexOutOfRangeException("Attempted to find more nearest neighbors than pieces exist");
            foreach (Piece a in Board)
            {
                Piece FurthestNearNeighbor = null;
                foreach (Piece b in Board)
                {
                    if (a == b) continue;
                    if (nn.Count < k)
                    {
                        nn.Add(b);
                        continue;
                    }
                    if (FurthestNearNeighbor == null)
                    {
                        float distance = 0;
                        foreach (Piece p in nn)
                        {
                            float new_distance = Vector3.Distance(p, a);
                            if (new_distance > distance)
                            {
                                distance = new_distance;
                                FurthestNearNeighbor = p;
                            }
                        }
                    }
                    if (Vector3.Distance(a, b) < Vector3.Distance(a, FurthestNearNeighbor))
                    {
                        nn.Remove(FurthestNearNeighbor);
                        nn.Add(b);
                        FurthestNearNeighbor = null;
                    }
                }
                NearestNeighbors.Add(a, nn);
                nn.Clear();
            }
        }
    }
}
