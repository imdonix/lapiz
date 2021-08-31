using UnityEngine;


public class PathFindRequest
{
    private Vector2Int start;
    private Vector2Int end;
    private Path path;
    private bool finished;
    private bool successful;

    public PathFindRequest(Vector2Int start, Vector2Int end)
    {
        this.start = start;
        this.end = end;
    }

    public Vector2Int GetStart()
    {
        return start;
    }

    public Vector2Int GetEnd()
    {
        return end;
    }

    public Path GetPath()
    {
        return path;
    }

    public bool IsSuccessful()
    {
        return successful;
    }

    public bool IsDone()
    {
        lock (this)
        {
            return finished;
        }
    }

    public  void FinalizePath(Path path)
    {
        lock (this)
        {
            this.finished = true;
            this.successful = true;
            this.path = path;
        }
    }

    public void FinalizePath()
    {
        lock (this)
        {
            this.finished = true;
            this.successful = false;
            this.path = null;
        }
    }
}

