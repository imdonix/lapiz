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

    protected override void Process(Item item)
    {
        item.transform.position = GetOutputLocation();
        item.ThrowAway((Vector3.up + Vector3.right) * 1600);
    }
}
