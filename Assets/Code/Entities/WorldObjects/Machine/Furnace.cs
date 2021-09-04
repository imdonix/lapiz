using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Furnace : Crafter
{

    private const string ID = "furnace";


    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {}

    protected override void RegisterCraftables()
    {
        foreach (ICraftable craftable in ItemLibrary.Instance.GetCraftablePrefs(this))
        {
            craftables.Add(craftable);
        }
    }

    public override string GetID()
    {
        return ID;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().Furnace;
    }
}

