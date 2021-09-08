using Photon.Pun;
using System;
using System.Collections.Generic;


public abstract class NPC : Ninja
{
    protected Job job;

    #region UNITY

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();


    }

    protected override void Update()
    {
        base.Update();

        if (PhotonNetwork.IsMasterClient)
        {
            if (ReferenceEquals(job, null) || job.IsOver())
                job = FindJob();
            else
                job.Update();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (PhotonNetwork.IsMasterClient)
        {
            if (!ReferenceEquals(job, null) && !job.IsOver())
                job.FixedUpdate();
        }
    }

    #endregion

    protected abstract Job FindJob();

    public void OverrideJob(Job job)
    {
        this.job = job;
    }

    public override void Equip(Tool item){}

}

