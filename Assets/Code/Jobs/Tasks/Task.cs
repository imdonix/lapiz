public abstract class Task
{
    private bool isDone = false;
    private bool isInited = false;
    private bool isFailed = false;

    protected readonly Ninja owner;

    protected Task(Ninja owner)
    {
        this.owner = owner;
    }

    public void Update()
    {
        if (!isDone)
        {
            if (!isInited)
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

    protected void Succeed()
    {
        isDone = true;
        isFailed = false;
    }

    protected void Fail()
    {
        isDone = true;
        isFailed = false;
    }

    public bool IsDone()
    {
        return isDone;
    }

    public bool IsFailed()
    {
        return isFailed;
    }

    protected virtual void DoInit() {}

    protected virtual void DoUpdate() {}

    protected virtual void DoFixed() {}
}

