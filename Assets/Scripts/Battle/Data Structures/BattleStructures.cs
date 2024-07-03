using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Resolvers;
using Unity.VisualScripting;
using UnityEngine;

// board dynamically updated with the flow of battle
public static class BattleBoard
{
    private static Dictionary<string, List<Piece>> Board = new Dictionary<string, List<Piece>>();

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

    public static List<Piece> K_Nearest(Piece p, int k, string perspective)
    {
        if (!Board.ContainsKey(perspective)) return null;
        Board[perspective].Sort(new PieceCompare(p));
        if (Board[perspective].Count < k)
        {
            Debug.Log("Trying to find more near neighbors than possible");
            k = Board[perspective].Count;
        }
        return Board[perspective].GetRange(0, k);
    }
}

    /*
    public static class KNN
    {
        public class KNNList
        {
            public Dictionary<string, List<Piece>> KNN = new Dictionary<string, List<Piece>>();

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

            foreach (List<Piece> side in Board.Values)
            {
                if (k > side.Count + 1) k = side.Count + 1;
                foreach (Piece a in side)
                {
                    Piece FurthestNearNeighbor = null;
                    foreach (string perspective in Board.Keys)
                    {
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
                        _kNNList.KNN[perspective] = new List<Piece>(nn);
                        nn.Clear();
                    }
                    NearestNeighbors.Add(a, _kNNList);
                }
            }
        }
    }
    
    public class QuadTree
    {
        QuadBranch root = new QuadBranch();

        public void Create(List<Piece> PieceBoard)
        {
            foreach (Piece piece in PieceBoard)
            {
                root.Insert(piece);
            }
        }
    }

    public abstract class QuadNode
    {
        public int quadrant;
        public QuadBranch parent;

        public abstract bool Insert(Piece piece);

        //public abstract List<Piece> kNearestNeighbors(Piece target, Dictionary<QuadNode, List<Piece>> soln);
    }

    public class QuadLeaf : QuadNode
    {
        public Piece first;
        public Piece second;

        public QuadLeaf(QuadBranch parent, int quadrant)
        {
            this.parent = parent;
            this.quadrant = quadrant;
        }

        public override bool Insert(Piece piece)
        {
            if (first == null)
            {
                first = piece;
                return true;
            }
            else if (second == null)
            {
                second = piece;
                return true;
            }
            else
            {
                return false;
            }
        }

        /*
        public override List<Piece> kNearestNeighbors(Piece target, List<Piece> soln = null, float closestNeighbor = float.PositiveInfinity)
        {
            return new List<Piece>();
        }
    }

    public class QuadBranch : QuadNode
    {
        public QuadNode[] children = new QuadNode[4]; // using mathematical graph quadrants (counter-clockwise starting top right)
        public QuadBoundary GridBoundary;

        public QuadBranch()
        {
            GridBoundary = new QuadBoundary(new Vector2(Boundary.Left, Boundary.Top), new Vector2(Boundary.Right, Boundary.Bottom));
        }

        public QuadBranch(QuadLeaf current, Piece piece, QuadBoundary boundary)
        {
            GridBoundary = boundary;
            Insert(current.first);
            Insert(current.second);
            Insert(piece);
        }

        // we do partitioning here!
        public override bool Insert(Piece piece)
        {
            byte quadrant;

            if (piece.Position.y > GridBoundary.MidPointY)
            {
                if (piece.Position.x > GridBoundary.MidPointX)
                {
                    quadrant = 0;
                }
                else
                {
                    quadrant = 1;
                }
            }
            else
            {
                if (piece.Position.x < GridBoundary.MidPointX)
                {
                    quadrant = 2;
                }
                else
                {
                    quadrant = 3;
                }
            }

            // only create memory of quadleaf if it's being used
            children[quadrant] ??= new QuadLeaf(this, quadrant);

            if (!children[quadrant].Insert(piece))      // a leaf is being inserted into and it is already occupied by two pieces; formally, we know Insert(Piece) can only return false for the QuadLeaf definition, thus the cast must always be valid.
            {
                QuadBoundary new_boundary = new QuadBoundary(GridBoundary, quadrant);
                children[quadrant] = new QuadBranch((QuadLeaf)children[quadrant], piece, new_boundary);
                children[quadrant].parent = this;
                children[quadrant].quadrant = quadrant;
            }

            return true;
        }
    }


    /// <summary>
    /// A QuadBoundary is canonically defined by two points at diagonally opposite edges which define a square which is split in the exact center horizontally and vertically into each leaf.
    /// </summary>
    public struct QuadBoundary
    {
        public Vector2 TopLeft;
        public Vector2 BottomRight;

        public float MidPointY;
        public float MidPointX;

        public QuadBoundary(Vector2 tl, Vector2 br)
        {
            TopLeft = tl;
            BottomRight = br;

            MidPointY = (TopLeft.y + BottomRight.y) / 2;
            MidPointX = (TopLeft.x + BottomRight.x) / 2;
        }

        public QuadBoundary(QuadBoundary old)
        {
            TopLeft = old.TopLeft;
            BottomRight = old.BottomRight;

            MidPointY = old.MidPointY;
            MidPointX = old.MidPointX;
        }

        public QuadBoundary(QuadBoundary old, byte quadrant) : this(old)
        {
            if (quadrant == 0)
            {
                TopLeft.Set(MidPointX, TopLeft.y);
                BottomRight.Set(BottomRight.x, MidPointY);
            }
            else if (quadrant == 1)
            {
                // TopLeft = TopLeft
                BottomRight.Set(MidPointX, MidPointY);
            }
            else if (quadrant == 2)
            {
                TopLeft.Set(TopLeft.x, MidPointY);
                BottomRight.Set(MidPointX, BottomRight.y);
            }
            else
            {
                TopLeft.Set(MidPointX, MidPointY);
                // BottomRight = BottomRight
            }

            MidPointY = (TopLeft.y + BottomRight.y) / 2;
            MidPointX = (TopLeft.x + BottomRight.x) / 2;
        }
    }
    */ 
