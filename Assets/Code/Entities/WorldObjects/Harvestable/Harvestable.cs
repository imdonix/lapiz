using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Harvestable : WorldObject, IHarvestable
{
    private int state = 0;

    public bool Harvest(LivingEntity harvester, HandTool tool, out Item reward)
    {
        reward = null;
        if (!IsCorrectTool(tool)) return false;

        this.state++;

        if (state >= GetRate())
        {
            Vector3 pos = transform.position + Vector3.up + (harvester.transform.position - transform.position).normalized * GetDropDistacne();
            reward = SpawnItem(pos);
            this.state = 0;
            return true;
        }
        return false;
    }

    private Item SpawnItem(Vector3 pos)
    {
        Item pref = GetReward();
        return PhotonNetwork.Instantiate(pref.name, pos, Quaternion.identity).GetComponent<Item>();
    }

    protected abstract Item GetReward();

    protected abstract int GetRate();

    protected abstract bool IsCorrectTool(HandTool tool);

    protected abstract float GetDropDistacne();

}

