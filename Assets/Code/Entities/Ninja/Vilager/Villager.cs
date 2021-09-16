using System.Collections;
using UnityEngine;


public class Villager : NPC
{

    protected override Job FindJob()
    {
        throw new System.NotImplementedException();
    }


    public override bool IsAlly()
    {
        return true;
    }

    public override bool IsVillager()
    {
        return true;
    }

    protected override void Die()
    {
    }
}
