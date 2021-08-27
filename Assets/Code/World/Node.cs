using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Node : IComparable<Node>
{
    public readonly int H;
    public readonly int G;
    public readonly bool startNode;
    public readonly Vector2Int position;
    public readonly Vector2Int parent;

    public Node(Vector2Int position, Vector2Int parent, int h, int g)
    {
        this.position = position;
        this.parent = parent;
        this.startNode = false;
        this.H = h;
        this.G = g;
    }

    public Node(Vector2Int position, int h, int g)
    {
        this.position = position;
        this.parent = Vector2Int.zero;
        this.startNode = true;
        this.H = h;
        this.G = g;
    }

    public int GetF()
    {
        return H + G;
    }
    public int CompareTo(Node n)
    {
        return GetF() - n.GetF();
    }
}
