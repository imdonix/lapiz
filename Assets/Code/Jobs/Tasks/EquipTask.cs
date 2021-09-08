using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EquipTask : Task
{
    private readonly ToolType type;

    public EquipTask(Ninja owner, ToolType type) : base(owner)
    {
        this.type = type;
    }

    protected override void DoInit()
    {
        owner.Swap(type);
        Succeed();
    }
}

