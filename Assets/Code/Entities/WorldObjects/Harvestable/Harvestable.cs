using Photon.Pun;
using UnityEngine;

public abstract class Harvestable : WorldObject, IHarvestable
{
    private int harvest = 0;


    public bool Harvest(LivingEntity harvester, int rate, HandTool tool, out Item reward)
    {
        reward = null;
        if (!IsCorrectTool(tool)) return false;

        this.harvest += rate;
        if (harvest >= GetRate())
        {
            Vector3 pos = transform.position + Vector3.up + (harvester.transform.position - transform.position).normalized * GetSize();
            reward = SpawnItem(pos);
            this.harvest = 0;
            this.GetProvider().DiscardJob();
            OnHarvested();
            return true;
        }

        return false;
    }

    public bool Harvest(LivingEntity harvester, HandTool tool, out Item reward)
    {
        return Harvest(harvester, 1, tool, out reward);
    }

    public bool IsCorrectTool(HandTool tool)
    {
        return tool.GetItemPref().Equals(GetCorrectTool());
    }


    private Item SpawnItem(Vector3 pos)
    {
        Item pref = GetReward();
        return PhotonNetwork.Instantiate(pref.name, pos, Quaternion.identity).GetComponent<Item>();
    }

    protected override int GetPriority()
    {
        return JobProvider.HARVESTABLE + Random.Range(0, 4);
    }

    public override Job GetJob(NPC npc)
    {
        return new HarvestJob(npc, this);
    }

    public override bool IsWorkAvailable()
    {
        return true;
    }

    public override bool HasTag()
    {
        return false;
    }

    protected abstract void OnHarvested();

    protected abstract Item GetReward();

    protected abstract int GetRate();

    public abstract Tool GetCorrectTool();


}

