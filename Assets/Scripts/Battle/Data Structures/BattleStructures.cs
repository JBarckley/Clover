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
    private static Dictionary<string, List<Piece>> Board;
    private static List<Piece> PlayerBoard = new List<Piece>();

    public static void Create(GameBoard GameBoard)
    {
        Board.Add("player", new List<Piece>());
        Board.Add("enemy", new List<Piece>());

        for (int i = 0; i < 8; i++)
        {
            for (int k = 0; k < 8; k++)
            {
                Piece piece = GameBoard[i, k];
                if (piece.Name != PieceName.None)
                {
                    if (i < 4)
                    {
                        Board["player"].Add(piece);
                    }
                    else
                    {
                        Board["enemy"].Add(piece);
                    }
                }
            }
        }
    }

    public static void Update()
    {
        foreach (Piece p in Board["player"])
        {
            p.Update();
        }
        foreach (Piece p in Board["enemy"])
        {
            p.Update();
        }
    }

    public static void PhysicsUpdate()
    {
        foreach (Piece p in Board["player"])
        {
            p.Update();
        }
        foreach (Piece p in Board["enemy"])
        {
            p.Update();
        }
    }

    public static Piece Find(Piece piece)
    {
        return piece; // not yet implemented
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

    public static class KNN
    {
        public class KNNList
        {
            public Dictionary<string, List<Piece>> KNN;

            public List<Piece> PlayerPieces { 
                get
                {
                    return KNN["player"];
                } 
            }

            public List<Piece> EnemyPieces { 
                get
                {
                    return KNN["enemy"];
                } 
            }

            public KNNList()
            {
                KNN.Add("player", new List<Piece>());
                KNN.Add("enemy", new List<Piece>());
            }
        }

        public static Dictionary<Piece, KNNList> NearestNeighbors = new Dictionary<Piece, KNNList>();

        public static bool AugmentKNN()
        {
            return true;
        }

        /// <summary>
        /// Finds the K-Nearest Neighbors for every piece on the board
        /// </summary>
        /// <param name="k">Amount of nearest neighbors</param>
        /// <param name="perspective">"enemy" gives nearest enemies, "player" gives nearest players</param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static void FindKNN(int k)
        {
            NearestNeighbors.Clear();
            List<Piece> nn = new List<Piece>();
            KNNList _kNNList = new KNNList();
            if (k > Board.Count + 1) throw new IndexOutOfRangeException("Attempted to find more nearest neighbors than pieces exist");
            foreach (List<Piece> side in Board.Values)
            {
                foreach (Piece a in side)
                {
                    Piece FurthestNearNeighbor = null;
                    foreach (string perspective in new List<string>{ "player", "enemy" }){
                        foreach (Piece b in Board[perspective])
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
                        _kNNList.KNN[perspective] = nn;
                    }
                    NearestNeighbors.Add(a, _kNNList);
                    nn.Clear();
                }
            }
        }
    }
}
