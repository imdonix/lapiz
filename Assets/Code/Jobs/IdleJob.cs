using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class IdleJob : Job
{
    public IdleJob(Ninja owner) : base(owner) {}

    protected override void BuildRoutine()
    {
        routine.Add(new IdleTask(owner));
    }
}

