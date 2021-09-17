using System;
using System.Collections;
using UnityEngine;


public class HarvestTask : Task
{
    private readonly Harvestable harvestable;

    public HarvestTask(Ninja owner, Harvestable harvestable) : base(owner)
    {
        this.harvestable = harvestable;
    }

    public Harvestable GetHarvestable()
    {
        return harvestable;
    }

    public void End(Item item)
    {
        item.Interact(owner);
        Succeed(item);
    }

    protected override void DoUpdate()
    {
        owner.SetTargetLook(harvestable.transform.position);
        owner.GetArms().Harvest(this);
    }

}
