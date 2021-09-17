using System.Collections;
using UnityEngine;


public class AcquireTask : Task
{
    private readonly Tool tool;

    public AcquireTask(Ninja owner, Tool tool) : base(owner)
    {
        this.tool = tool;
    }

    protected override void DoInit()
    {
        base.DoInit();
        owner.GetInventory().Equip(tool);
        Succeed();
    }
}
