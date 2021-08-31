using MinMaxHeap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private const int MIPU = 500;
    private const int RAN_MP = 4;

    [Header("World")]
    [SerializeField] private Vector2Int Grid;
    [SerializeField] private float Size;
    [SerializeField] private float Height;

    [Header("Debug")]
    [SerializeField] private bool DebugEnabled;
    [SerializeField] private bool CollisionMapDebug;
    [SerializeField] private Path LastPathDebug;

    private bool loaded;
    private bool[] walkable;
        

    #region UNITY

    private void Awake()
    {
        Init();
        StartCoroutine(LoadPathFindTerrain());
    }

    #endregion

    public bool IsInsideGridPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < Grid.x && pos.y < Grid.y;
    }

    /// <summary>
    /// Convert world position into grid position.
    /// </summary>
    public Vector2Int GetGridPosition(Vector3 realPos)
    {
        return new Vector2Int(Mathf.RoundToInt(realPos.x / Size), Mathf.RoundToInt(realPos.z / Size));
    }

    public PathFindRequest Request(Vector2Int from, Vector2Int to)
    {
        PathFindRequest request = new PathFindRequest(from, to);
        StartCoroutine(PathFind(request));
        return request;
    }

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

                if (check > MIPU)
                {
                    check = 0;
                    yield return new WaitForFixedUpdate();
                }
            }
        loaded = true;
    }

    private IEnumerator PathFind(PathFindRequest req)
    {
        yield return new WaitUntil(() => loaded);

        MinHeap<Node> queue = new MinHeap<Node>();
        Node[,] visited = new Node[Grid.x, Grid.y];

        Vector2Int from = req.GetStart();
        Vector2Int to = req.GetEnd();

        if (!IsInsideGridPosition(from))
        {
            req.FinalizePath();
            yield break;
        } 

        int iterations = 0;
        Node endNode = null;

        queue.Add(new Node(from, 0, 0, Size));
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

            if (iterations > MIPU / 2)
            {
                iterations = 0;
                yield return new WaitForFixedUpdate();
            }
        }

        if (endNode != null)
        {
            Path final = CreatePath(endNode, visited);
            LastPathDebug = final;
            req.FinalizePath(final);
        }
        else
            req.FinalizePath();
    }

    private Node[] Neighbour(Node from, Vector2Int target)
    {
        Node[] tmp = new Node[8];
        int random = Random.Range(-RAN_MP, RAN_MP);

        Vector2Int up = from.position + Vector2Int.up;
        Vector2Int down = from.position + Vector2Int.down;
        Vector2Int left = from.position + Vector2Int.left;
        Vector2Int right = from.position + Vector2Int.right;
        Vector2Int leftUp = from.position + Vector2Int.left + Vector2Int.up;
        Vector2Int leftDown = from.position + Vector2Int.left + Vector2Int.down;
        Vector2Int rightUp = from.position + Vector2Int.right + Vector2Int.up;
        Vector2Int rightDown = from.position + Vector2Int.right + Vector2Int.down;

        tmp[0] = new Node(up, from.position, Manhattan(up, target), from.G + 10 + random, Size);
        tmp[1] = new Node(down, from.position, Manhattan(down, target), from.G + 10 + random, Size);
        tmp[2] = new Node(left, from.position, Manhattan(left, target), from.G + 10 + random, Size);
        tmp[3] = new Node(right, from.position, Manhattan(right, target), from.G + 10 + random, Size);
        tmp[4] = new Node(leftUp, from.position, Manhattan(right, target), from.G + 14 + random, Size);
        tmp[5] = new Node(leftDown, from.position, Manhattan(right, target), from.G + 14 + random, Size);
        tmp[6] = new Node(rightUp, from.position, Manhattan(right, target), from.G + 14 + random, Size);
        tmp[7] = new Node(rightDown, from.position, Manhattan(right, target), from.G + 14 + random, Size);

        return tmp;
    }

    private Path CreatePath(Node node, Node[,] map)
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

    private static int Manhattan(Vector2Int pos, Vector2Int target)
    {
        return Mathf.Abs(pos.x - target.x) + Mathf.Abs(pos.y - target.y);
    }

    private bool IsWalkable(Node n)
    {
        Vector2Int pos = n.position;
        return walkable[PosToArray(pos.x, pos.y)];
    }

    private bool IsNodeInside(Node n)
    {
        Vector2Int p = n.position;
        return p.x < Grid.x && p.x >= 0 && p.y < Grid.y && p.y >= 0;
    }


    private void OnDrawGizmos()
    {
        if (!DebugEnabled || !loaded) return;

        if(CollisionMapDebug)
            for (int x = 0; x < Grid.x; x++)
                for (int y = 0; y < Grid.y; y++)
                {
                    Gizmos.color = walkable[PosToArray(x, y)] ? Color.white : Color.red;
                    Gizmos.DrawCube(new Vector3(Size * x, Height, Size * y), Vector3.one * Size / 1.25F);
                }

        if (LastPathDebug != null)
        {
            Gizmos.color = Color.blue;
            Path clone = (Path) LastPathDebug.Clone();

            float i = .25F;
            float inc = .75F / clone.GetLenght();
            while (clone.HasNext())
            {
                Vector3 pos = clone.Current().GetRealPos();
                Gizmos.DrawCube(pos, Vector3.one * i );
                clone.Next();
                i += inc;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawCube(LastPathDebug.Current().GetRealPos(), Vector3.one * 2);
        }
    }
}
