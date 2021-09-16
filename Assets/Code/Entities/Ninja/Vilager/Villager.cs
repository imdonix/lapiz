using Photon.Pun;
using System.Collections;
using UnityEngine;


public class Villager : NPC
{
    protected override void Start()
    {
        base.Start();

        if (PhotonNetwork.IsMasterClient)
        {
            Story.Loaded.population.Populate(this);
            RequestBadge(Village.None);
        }
    }

    protected override Job FindJob()
    {
        return new IdleJob(this);
    }

    public override bool IsAlly()
    {
        return true;
    }

    public override bool IsVillager()
    {
        return true;
    }

    public override void Alert(LivingEntity source)
    {
        OverrideJob(new RunAwayJob(this, source));
    }

    protected override void Die()
    {
        DropInventoryItems();
        Story.Loaded.population.RegisterDead(this);
        PhotonNetwork.Destroy(photonView);
    }
}
