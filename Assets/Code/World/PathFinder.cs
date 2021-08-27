using MinMaxHeap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private const int MAX_ITERATION_PER_UPDATE = 500;

    [Header("World")]
    [SerializeField] private Vector2Int Grid;
    [SerializeField] private float Size;
    [SerializeField] private float Height;

    [Header("Debug")]
    [SerializeField] private bool DebugEnabled;
    [SerializeField] private bool CollisionMap;

    private bool loaded;
    private bool[] walkable;

    #region UNITY

    private void Awake()
    {
        Init();
        StartCoroutine(LoadPathFindTerrain());
    }

    #endregion

    private void Init()
    {
        walkable = new bool[Grid.x * Grid.y];
        loaded = false;
    }

    private int PosToArray(int x, int y)
    {
        return x * Grid.y + y;
    }

    private IEnumerator LoadPathFindTerrain()
    {
        int terrainMask = ~(1 << 7);
        int check = 0;

        for (int x = 0; x < Grid.x; x++)
            for (int y = 0; y < Grid.y; y++)
            {
                walkable[PosToArray(x, y)] = !Physics.CheckBox(new Vector3(Size * x, Height, Size * y), Vector3.one * Size, Quaternion.identity, terrainMask);
                check++;

                if (check > MAX_ITERATION_PER_UPDATE)
                {
                    check = 0;
                    yield return new WaitForFixedUpdate();
                }
            }
        loaded = true;
    }

    private IEnumerator PathFind(PathFindRequest req)
    {
        MinHeap<Node> queue = new MinHeap<Node>();
        Node[,] visited = new Node[Grid.x, Grid.y];

        Node endNode = null;
        Vector2Int from = req.GetStart();
        Vector2Int to = req.GetEnd();
        int iterations = 0;

        queue.Add(new Node(from, 0, 0));
        while (queue.Count > 0)
        {
            Node node = queue.ExtractMin();
            visited[node.position.x, node.position.y] = node;

            if (node.position == to) 
            {
                endNode = node;
                break;
            }


            foreach (Node n in Neighbour(node, to))
                if (IsNodeInside(n) && IsWalkable(n))
                {
                    Node old = visited[n.position.x, n.position.y];
                    if (old == null || (n.GetF() < old.GetF() && old.parent != n.position))
                    {
                        visited[n.position.x, n.position.y] = n;
                        queue.Add(n);
                    }
                }

            if (iterations > MAX_ITERATION_PER_UPDATE / 2)
            {
                iterations = 0;
                yield return new WaitForFixedUpdate();
            }
        }

        if (endNode != null)
            req.FinalizePath(CreatePath(endNode, visited));
        else
            req.FinalizePath();
    }

    private Node[] Neighbour(Node from, Vector2Int target)
    {
        Node[] tmp = new Node[4];
        Vector2Int up = from.position + Vector2Int.up;
        tmp[0] = new Node(up, from.position, Manhattan(up, target), from.G + 1);
        Vector2Int down = from.position + Vector2Int.down;
        tmp[1] = new Node(down, from.position, Manhattan(down, target), from.G + 1);
        Vector2Int left = from.position + Vector2Int.left;
        tmp[2] = new Node(left, from.position, Manhattan(left, target), from.G + 1);
        Vector2Int right = from.position + Vector2Int.right;
        tmp[3] = new Node(right, from.position, Manhattan(right, target), from.G + 1);
        return tmp;
    }

    private Path CreatePath(Node node, Node[,] map)
    {
        List<Node> nodes = new List<Node>();
        nodes.Add(map[node.position.x, node.position.y]);

        while (node.parent != null)
        {
            node = map[node.parent.x, node.parent.y];
            nodes.Add(map[node.position.x, node.position.y]);
        }

        return new Path(nodes);
    }

    private static int Manhattan(Vector2Int pos, Vector2Int target)
    {
        return Mathf.Abs(pos.x - target.x) + Mathf.Abs(pos.y - target.y);
    }

    private bool IsWalkable(Node n)
    {
        Vector2Int pos = n.position;
        return walkable[PosToArray(pos.x, pos.y)];
    }

    public PathFindRequest Request(Vector2Int from, Vector2Int to)
    {
        PathFindRequest request = new PathFindRequest(from, to);
        StartCoroutine(PathFind(request));
        return request;
    }

    private bool IsNodeInside(Node n)
    {
        Vector2Int p = n.position;
        return p.x < Grid.x && p.x >= 0 && p.y < Grid.y && p.y >= 0;
    }


    private void OnDrawGizmos()
    {
        if (!DebugEnabled || !loaded) return;

        for (int x = 0; x < Grid.x; x++)
            for (int y = 0; y < Grid.y; y++)
            {
                Gizmos.color = walkable[PosToArray(x, y)] ? Color.white : Color.red;
                Gizmos.DrawCube(new Vector3(Size * x, Height, Size * y), Vector3.one * Size);
            }
    }
}
