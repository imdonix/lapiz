using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    private const int MAX_ITERATION_PER_UPDATE = 500;

    [Header("World")]
    [SerializeField] private Vector2Int Grid;
    [SerializeField] private float Size;
    [SerializeField] private float Height;

    [Header("Debug")]
    [SerializeField] private bool DebugEnabled;

    private bool loaded;
    private bool[] terrain;

    #region UNITY

    private void Awake()
    {
        Init();
        StartCoroutine(LoadPathFindTerrain());
    }

    #endregion

    private void Init()
    {
        loaded = false;
        terrain = new bool[Grid.x * Grid.y];
    }

    private IEnumerator LoadPathFindTerrain()
    {
        int terrainMask = ~(1 << 7);
        int check = 0;

        for (int x = 0; x < Grid.x; x++)
            for (int y = 0; y < Grid.y; y++)
            {
                terrain[PosToTerrain(x, y)] = Physics.CheckBox(new Vector3(Size * x, Height, Size * y), Vector3.one * Size, Quaternion.identity, terrainMask);
                check++;

                if (check > MAX_ITERATION_PER_UPDATE) 
                {
                    check = 0;
                    yield return new WaitForFixedUpdate();
                }
            }

        loaded = true;
    }


    private int PosToTerrain(int x, int y)
    {
        return x * Grid.y + y;
    }

    private void OnDrawGizmos()
    {
        if (!DebugEnabled || !loaded) return;

        for (int x = 0; x < Grid.x; x++)
            for (int y = 0; y < Grid.y; y++)
            {
                Gizmos.color = terrain[PosToTerrain(x, y)] ? Color.red : Color.white;
                Gizmos.DrawCube(new Vector3(Size * x, Height, Size * y), Vector3.one * Size);
            }
    }
}
