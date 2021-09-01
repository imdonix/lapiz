using MinMaxHeap;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private const int MIPU = 150;


    [Header("World")]
    [SerializeField] private Vector2Int Grid;
    [SerializeField] private float Size;
    [SerializeField] private float Height;

    [Header("Debug")]
    [SerializeField] private bool DebugEnabled;
    [SerializeField] private bool CollisionMapDebug;
   

    private bool loaded;
    private bool[] walkable;

    private PathFindRequest lastRequest;

    #region UNITY

    private void Awake()
    {
        Init();
    }

    #endregion

    /// <summary>
    /// Reload the collision map for pathfinding.
    /// </summary>
    public void ReloadTerrain()
    {
        StartCoroutine(LoadPathFindTerrain());
    }

    /// <summary>
    /// Convert world position into grid position.
    /// </summary>
    public Vector2Int GetGridPosition(Vector3 realPos)
    {
        return new Vector2Int(Mathf.RoundToInt(realPos.x / Size), Mathf.RoundToInt(realPos.z / Size));
    }

    /// <summary>
    /// Get a walkable pos around a object. DUMMY alg use only for WorldObjects. Return the same pos if not found.
    /// </summary>
    public Vector2Int GetNearestWalkablePosition(Vector2Int pos)
    {
        Vector2Int dir = Vector2Int.up;
        Vector2Int it = pos;
        for (int i = 0; i < 8; i++)
        { 
            if (walkable[PosToArray(Grid, it.x, it.y)]) 
                return it;
            it += dir;
        }
        return pos;
    }


    public PathFindRequest Request(Vector2Int from, Vector2Int to)
    {
        PathFindRequest request = new PathFindRequest(from, to);

        object[] args = new object[4] { request, walkable, Grid, Size };
        Thread finder = new Thread(PathFindJob.PathFind);
        finder.Start(args);

        lastRequest = request;
        return request;
    }

    private void Init()
    {
        walkable = new bool[Grid.x * Grid.y];
        loaded = false;
    }

    private IEnumerator LoadPathFindTerrain()
    {
        int terrainMask = ~(1 << 7);
        int check = 0;

        for (int x = 0; x < Grid.x; x++)
            for (int y = 0; y < Grid.y; y++)
            {
                walkable[PosToArray(Grid, x, y)] = !Physics.CheckBox(new Vector3(Size * x, Height, Size * y), Vector3.one * Size, Quaternion.identity, terrainMask);
                check++;

                if (check > MIPU)
                {
                    check = 0;
                    yield return new WaitForFixedUpdate();
                }
            }
        loaded = true;
    }

    public static int PosToArray(Vector2Int grid, int x, int y)
    {
        return x * grid.y + y;
    }

    public static bool IsInsideGridPosition(Vector2Int pos, Vector2Int grid)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < grid.x && pos.y < grid.y;
    }

    #region GIZMO


    #endregion

    private void OnDrawGizmos()
    {
        if (!DebugEnabled || !loaded) return;

        if(CollisionMapDebug)
            for (int x = 0; x < Grid.x; x++)
                for (int y = 0; y < Grid.y; y++)
                {
                    Gizmos.color = walkable[PosToArray(Grid, x, y)] ? Color.white : Color.red;
                    Gizmos.DrawCube(new Vector3(Size * x, Height, Size * y), Vector3.one * Size / 1.25F);
                }

        if (lastRequest != null && lastRequest.IsDone() && lastRequest.IsSuccessful())
        {
            Gizmos.color = Color.blue;
            Path clone = (Path)lastRequest.GetPath().Clone();

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
            Gizmos.DrawCube(lastRequest.GetPath().Current().GetRealPos(), Vector3.one * 2);
        }
    }
}
