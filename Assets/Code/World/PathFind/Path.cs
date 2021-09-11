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
        this.i = 0;
    }

    public int GetLenght()
    {
        return path.Count;
    }

    public bool HasNext()
    {
        return path.Count - 1 > i;
    }

    public Node Current()
    {
        return path[path.Count - 1 - i];
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

