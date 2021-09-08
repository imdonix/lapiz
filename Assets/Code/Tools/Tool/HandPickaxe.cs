using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HandPickaxe : HandTool
{
    public override float GetSpeed()
    {
        return 0.75F;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.PickaxePref;
    }
}

