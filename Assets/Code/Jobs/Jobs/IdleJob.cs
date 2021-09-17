using UnityEngine;


public class IdleJob : Job
{
    public IdleJob(Ninja owner) : base(owner) {}

    protected override void BuildRoutine()
    {
        routine.Add(new GoRandomPlaceTask(owner));
        routine.Add(new IdleTask(owner, Random.Range(2,5)));
    }
}

