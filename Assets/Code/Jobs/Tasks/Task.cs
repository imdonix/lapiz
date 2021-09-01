public abstract class Task
{
    private bool isDone = false;
    private bool isInited = false;

    public void Update()
    {
        if (!isDone)
        {
            if (isInited)
            {
                DoInit();
                isInited = true;
            }
            else
            {
                DoUpdate();
            }
        }
    }

    public void FixedUpdate()
    {
        if (!isDone)
        {
            DoFixed();
        }
    }

    public virtual void Reset()
    {
        this.isInited = false;
        this.isDone = false;
    }

    protected void Finish()
    {
        isDone = true;
    }

    public bool IsDone()
    {
        return isDone;
    }

    protected abstract void DoInit();

    protected abstract void DoUpdate();

    protected abstract void DoFixed();
}

