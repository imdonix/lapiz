using UnityEngine;


public class World : MonoBehaviour
{
    public static World Loaded;

    private PathFinder pathFinder;

    #region UNITY

    private void Awake()
    {
        this.pathFinder = GetComponent<PathFinder>();

        Loaded = this;
    }

    #endregion

    public PathFinder GetPathFinder()
    {
        return pathFinder;
    }

}
