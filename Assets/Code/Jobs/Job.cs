using System.Collections.Generic;

public abstract class Job
{
    protected readonly Ninja owner;
    protected readonly List<Task> routine;
    private int step;
    private bool init;

    protected Job(Ninja owner)
    {
        this.owner = owner;
        this.step = 0;
        this.routine = new List<Task>();
        this.init = false;
    }

    protected abstract void BuildRoutine();

    #region UNITY

    public void Update()
    {
        if (!init) 
        {
            BuildRoutine();
            init = true;
        }

        if (routine[step].IsDone())
        {
            step = (step + 1) % routine.Count;

            if (step == 0)
                foreach (Task task in routine)
                    task.Reset();
        }

        routine[step].Update();
    }

    public void FixedUpdate()
    {
        if(init)
            routine[step].FixedUpdate();
    }

    #endregion

}

