using System;

using UnityEngine;

public class HandAxe : HandTool
{
    public override float GetSpeed()
    {
        return 1F;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.AxePref;
    }
}

