using System.Collections.Generic;
using UnityEngine;


public class Path
{
    private readonly List<Node> path;
    private int i;

    public Path(List<Node> path)
    {
        this.path = path;
        this.path.Reverse();
        this.i = 0;
    }

    public bool HasNext()
    {
        return path.Count > i;
    }

    public Node Current()
    {
        return path[i];
    }

    public void Next()
    {
        i++;
    }
}

