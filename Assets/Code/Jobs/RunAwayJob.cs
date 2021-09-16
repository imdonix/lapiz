using System.Collections;
using UnityEngine;


public class RunAwayJob : Job
{
    private const float RUN_DISTANCE = 15;
    private readonly Entity attacker;

    public RunAwayJob(Ninja owner, Entity attacker) : base(owner) 
    {
        this.attacker = attacker;
    }

    protected override void BuildRoutine()
    {
        routine.Add(new GoTask(owner, SafePlace(attacker)));
    }


    private Vector3 SafePlace(Entity attacker)
    {
        Vector3 dir = TTT.Direction(attacker.transform.position, owner.transform.position);
        return owner.transform.position + (-dir * RUN_DISTANCE);

    }
}
