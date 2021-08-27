using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Projectile : Entity
{
    private const float LIFE_TIME = 10F;

    protected float timer;

    #region UNITY

    protected override void Update()
    {
        base.Awake();

        if (!photonView.IsMine) return;

        if (timer > LIFE_TIME)
            DestroyItem();

        timer += Time.deltaTime;
    }

    #endregion

    private void DestroyItem()
    {
        PhotonNetwork.Destroy(photonView);
    }
}
