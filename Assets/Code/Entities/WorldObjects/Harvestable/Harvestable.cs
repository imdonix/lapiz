using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Harvestable : WorldObject, IDamagable
{
    private int state = 0;

    public bool Harvest(LivingEntity harvester, out Item reward)
    {
        this.state++;

        if (state >= GetRate())
        {
            Vector3 pos = transform.position + Vector3.up + (harvester.transform.position - transform.position).normalized * GetDropDistacne();
            reward = SpawnItem(pos);
            this.state = 0;
            return true;
        }
        reward = null;
        return false;
    }

    private Item SpawnItem(Vector3 pos)
    {
        Item pref = GetReward();
        return PhotonNetwork.Instantiate(pref.name, pos, Quaternion.identity).GetComponent<Item>();
    }

    public void Damage(LivingEntity source, float damage)
    {
        Item tmp;
        Harvest(source, out tmp);
    }

    protected abstract Item GetReward();

    protected abstract int GetRate();

    protected abstract float GetDropDistacne();

}

