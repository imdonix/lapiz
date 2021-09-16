using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Chunin : Invader 
{
    #region UNITY

    protected override void Awake()
    {
        base.Awake();
    }

    #endregion

    protected override void SetupInventory()
    {
        photonView.RPC("SetBadge", Photon.Pun.RpcTarget.AllBuffered, (int)Village.Kerth);

        inventory.Equip(ItemLibrary.Instance.SwordPref);
    }

    protected override void SetupLootTable()
    {
        LootTable.Add(ItemLibrary.Instance.LapizPref);
        LootChance.Add(0.15F);
        LootAmount.Add(1);
    }
}

