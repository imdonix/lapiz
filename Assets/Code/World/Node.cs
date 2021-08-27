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
    public readonly float size;
    public readonly Vector2Int position;
    public readonly Vector2Int parent;

    public Node(Vector2Int position, Vector2Int parent, int h, int g, float size)
    {
        this.position = position;
        this.parent = parent;
        this.size = size;
        this.startNode = false;
        this.H = h;
        this.G = g;
    }

    public Node(Vector2Int position, int h, int g, float size)
    {
        this.position = position;
        this.parent = Vector2Int.zero;
        this.startNode = true;
        this.size = size;
        this.H = h;
        this.G = g;
    }

    public int GetF()
    {
        return H + G;
    }

    public Vector3 GetRealPos()
    {
        float half = size / 2;
        return new Vector3(position.x * size - half, 0, position.y * size - half);
    }

    public int CompareTo(Node n)
    {
        return GetF() - n.GetF();
    }
}
