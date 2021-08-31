using MinMaxHeap;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PathFindJob
{
    private const int RAN_MP = 2;

    public static void PathFind(object args)
    {
        object[] param = (object[]) args;

        PathFindRequest req = (PathFindRequest) param[0];
        bool[] walkable = (bool[]) param[1];
        Vector2Int grid = (Vector2Int) param[2];
        float size = (float) param[3];

        MinHeap<Node> queue = new MinHeap<Node>();
        Node[,] visited = new Node[grid.x, grid.y];

        Vector2Int from = req.GetStart();
        Vector2Int to = req.GetEnd();

        if (!PathFinder.IsInsideGridPosition(from, grid))
        {
            req.FinalizePath();
            return;
        }

        Node endNode = null;

        queue.Add(new Node(from, 0, 0, size));
        while (queue.Count > 0)
        {
            Node node = queue.ExtractMin();
            visited[node.position.x, node.position.y] = node;

            if (node.position == to)
            {
                endNode = node;
                break;
            }

            foreach (Node n in Neighbour(node, to, size))
                if (IsNodeInside(grid, n) && IsWalkable(walkable, grid, n))
                {
                    Node old = visited[n.position.x, n.position.y];
                    if (old == null || (n.GetF() < old.GetF() && old.parent != n.position))
                    {
                        visited[n.position.x, n.position.y] = n;
                        queue.Add(n);
                    }
                }
        }

        if (endNode != null)
        {
            Path final = CreatePath(endNode, visited);
            req.FinalizePath(final);
        }
        else
            req.FinalizePath();
    }

    private static Node[] Neighbour(Node from, Vector2Int target, float size)
    {
        Node[] tmp = new Node[8];

        Vector2Int up = from.position + Vector2Int.up;
        Vector2Int down = from.position + Vector2Int.down;
        Vector2Int left = from.position + Vector2Int.left;
        Vector2Int right = from.position + Vector2Int.right;
        Vector2Int leftUp = from.position + Vector2Int.left + Vector2Int.up;
        Vector2Int leftDown = from.position + Vector2Int.left + Vector2Int.down;
        Vector2Int rightUp = from.position + Vector2Int.right + Vector2Int.up;
        Vector2Int rightDown = from.position + Vector2Int.right + Vector2Int.down;

        tmp[0] = new Node(up, from.position, Manhattan(up, target), from.G + 10 , size);
        tmp[1] = new Node(down, from.position, Manhattan(down, target), from.G + 10 , size);
        tmp[2] = new Node(left, from.position, Manhattan(left, target), from.G + 10 , size);
        tmp[3] = new Node(right, from.position, Manhattan(right, target), from.G + 10 , size);
        tmp[4] = new Node(leftUp, from.position, Manhattan(right, target), from.G + 13 , size);
        tmp[5] = new Node(leftDown, from.position, Manhattan(right, target), from.G + 13, size);
        tmp[6] = new Node(rightUp, from.position, Manhattan(right, target), from.G + 13, size);
        tmp[7] = new Node(rightDown, from.position, Manhattan(right, target), from.G + 13, size);

        return tmp;
    }

    private static bool IsWalkable(bool[] walkable, Vector2Int grid, Node n)
    {
        Vector2Int pos = n.position;
        return walkable[PathFinder.PosToArray(grid, pos.x, pos.y)];
    }

    private static bool IsNodeInside(Vector2Int grid, Node n)
    {
        Vector2Int p = n.position;
        return p.x < grid.x && p.x >= 0 && p.y < grid.y && p.y >= 0;
    }

    private static int Manhattan(Vector2Int pos, Vector2Int target)
    {
        return Mathf.Abs(pos.x - target.x) + Mathf.Abs(pos.y - target.y);
    }

    private static Path CreatePath(Node node, Node[,] map)
    {
        List<Node> nodes = new List<Node>();
        nodes.Add(node);

        while (!node.startNode)
        {
            node = map[node.parent.x, node.parent.y];
            nodes.Add(node);
        }

        return new Path(nodes);
    }
}

