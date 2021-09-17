using Photon.Pun;
using System.Collections;
using UnityEngine;


public class Villager : NPC, IInteractable
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
        var jobs = World.Loaded.GetJobs();
        JobProvider provider = null;
        foreach (var job in jobs)
        {
            if (job.IsAvailable())
            {
                if (ReferenceEquals(provider, null))
                    provider = job;
                else if (provider.GetPriority() < job.GetPriority())
                    provider = job;
            }
        }

        if (ReferenceEquals(provider, null))
        {
            this.provider = null;
            return new IdleJob(this);
        }
        else
        {
            this.provider = provider;
            return provider.ApplyJob(this); ;
        }
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
        this.provider = null;
        OverrideJob(new RunAwayJob(this, source));
    }

    protected override void Die()
    {
        DropInventoryItems();
        Story.Loaded.population.RegisterDead(this);
        PhotonNetwork.Destroy(photonView);
    }

    public void Interact(Ninja source)
    {
        Scare(source);
    }

    public bool CanInteract()
    {
        return true;
    }

    public string GetDescription()
    {
        return Manager.Instance.GetLanguage().Scare;
    }

    #region PUN

    [PunRPC]
    public void OnScare(int id) 
    {
        foreach (LivingEntity entity in LivingEntity.GetAllies())
            if (entity.photonView.ViewID.Equals(id))
            {
                Alert(entity);
                break;
            }   
    }

    public void Scare(LivingEntity from)
    {
        photonView.RPC("OnScare", photonView.Owner, from.photonView.ViewID);
    }

    #endregion
}
