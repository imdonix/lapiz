using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Craft
{
    public readonly static List<Craft> All = new List<Craft>();

    public readonly ICraftable Craftable;
    public readonly Crafter Crafter;
    private float timer;

    private Craft(ICraftable craftable, Crafter crafter)
    {
        this.Craftable = craftable;
        this.Crafter = crafter;
        this.timer = 0;
        All.Add(this);
    }

    public void Update()
    {
        timer += Time.deltaTime;
    }

    public void End()
    {
        All.Remove(this);
    }

    public bool IsDone()
    {
        return Craftable.GetTime() < timer; 
    }

    public static Craft Start(ICraftable craftable, Crafter crafter)
    {
        return new Craft(craftable, crafter);
    }
}

