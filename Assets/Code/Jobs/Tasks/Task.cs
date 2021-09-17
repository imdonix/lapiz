using System;
using UnityEngine;

public abstract class Task
{
    private bool isDone = false;
    private bool isInited = false;
    private bool isFailed = false;

    private Item carried = null;

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

    public void Init(Item carried)
    {
        this.carried = carried;
        Debug.Log("I got " + carried);
    }

    protected void Succeed()
    {
        isDone = true;
        isFailed = false;
        carried = null;
    }

    public Item GetCarried()
    {
        return carried;
    }

    protected void Succeed(Item item)
    {
        isDone = true;
        isFailed = false;
        carried = item;
    }

    protected void Fail()
    {
        isDone = true;
        isFailed = false;
        carried = null;
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

