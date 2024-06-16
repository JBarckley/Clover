using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
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
     *      realized the size of my data set meant that brute force would either be the quickest or a very competitive
     *      strategy for a k nearest neighbors search
     */

    public static class BoardUtility
    {
        public static Dictionary<Piece, List<Piece>> NearestNeighbors = new Dictionary<Piece, List<Piece>>();

        public static bool AugmentKNN()
        {
            return true;
        }

        /*
        public static List<Piece> FindKNN2(int k, Piece piece)
        {
            NearestNeighborComparer nnkComparer = new NearestNeighborComparer();
            nnkComparer.CompareTo(piece);
            Board.Sort(nnkComparer);
            return Board.GetRange(0, k);
        }
        */

        public static void FindKNN(int k)
        {
            List<Piece> nn = new List<Piece>();
            if (k > Board.Count + 1) throw new IndexOutOfRangeException("Attempted to find more nearest neighbors than pieces exist");
            foreach (Piece a in Board)
            {
                foreach (Piece b in Board)
                {
                    if (a == b) continue;
                    if (nn.Count < k)
                    {
                        nn.Add(b);
                        continue;
                    }
                    if ((b.Position - a.Position).magnitude < (nn[k - 1].Position - a.Position).magnitude)
                    {
                        nn.RemoveAt(k - 1);
                        nn.Add(b);
                    }
                }
                NearestNeighbors.Add(a, nn);
                nn.Clear();
            }
        }
    }
}

/*
public class NearestNeighborComparer : IComparer<Piece>
{
    private Piece comparator;

    public int Compare(Piece x, Piece y)
    {
        if (comparator == null)
        {
            throw new ArgumentNullException("no comparison piece set in nearest neighbor search");
        }

        Vector3 comparedPos = comparator.Position;
        return (x.Position - comparedPos).magnitude.CompareTo((y.Position - comparedPos).magnitude);
    }

    public void CompareTo(Piece piece)
    {
        comparator = piece;
    }
}
*/
