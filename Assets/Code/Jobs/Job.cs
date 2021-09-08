using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job
{
    protected readonly Ninja owner;
    protected readonly List<Task> routine;
    private int index;
    private bool init;
    private bool finished;

    protected Job(Ninja owner)
    {
        this.owner = owner;
        this.routine = new List<Task>();
        this.index = 0;
        this.init = this.finished = false;
    }

    protected abstract void BuildRoutine();

    #region UNITY

    public void Update()
    {
        if (finished) return;

        Init();

        Task rt = routine[index];
        if (rt.IsDone())
        {

            if (rt.IsFailed())
            {
                finished = true;
            }
            else
            {
                if (index < routine.Count - 1)
                {
                    index++;
                }
                else
                {
                    finished = true;
                }
            }
        }
        else
            rt.Update();
    }

    public void FixedUpdate()
    {
        if(init && !routine[index].IsDone())
            routine[index].FixedUpdate();
    }

    #endregion

    public bool IsOver()
    {
        return finished;
    }

    private void Init()
    {
        if (init) return;
        BuildRoutine();
        init = true;
    }
}

