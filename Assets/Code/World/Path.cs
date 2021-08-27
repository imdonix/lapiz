using System.Collections.Generic;
using UnityEngine;
using System;

public class Path : ICloneable
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
        return path.Count - 1 > i;
    }

    public Node Current()
    {
        return path[i];
    }

    public void Next()
    {
        i++;
    }

    public object Clone()
    {
        return new Path(path);
    }
}

