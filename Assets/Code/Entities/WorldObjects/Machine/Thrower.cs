using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Thrower : Machine
{
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){}

    protected override void Store(Item item)
    {
        item.transform.position = GetOutputLocation();
        item.ThrowAway((Vector3.up + Vector3.right) * 1600);
    }

    protected override void Process(){}

    protected override void ResetInput(){}

    public override float GetSize()
    {
        return 2F;
    }

    protected override int GetPriority()
    {
        return JobProvider.THROWER;
    }

    public override Job GetJob(NPC npc)
    {
        throw new NotImplementedException();
    }


}
