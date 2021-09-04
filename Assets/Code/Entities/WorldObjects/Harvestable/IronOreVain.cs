using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class IronOreVain : Harvestable
{
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){}

    protected override float GetDropDistacne()
    {
        return 4.5F;
    }

    protected override int GetRate()
    {
        return 2;
    }

    protected override Item GetReward()
    {
        return ItemLibrary.Instance.IronOrePref;
    }
}


