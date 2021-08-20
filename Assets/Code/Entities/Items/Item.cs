﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Photon.Pun;

public abstract class Item : Entity, IInteractable
{
    protected float lifeTime;

    #region PRIVATE

    protected virtual void Update()
    {
        if (!photonView.IsMine) return;

        if (GetLifeTime() < lifeTime)
            DestroyItem();

        lifeTime += Time.deltaTime;
    }

    #endregion

    #region PRIVATE
    
    private void DestroyItem()
    {
        PhotonNetwork.Destroy(photonView);
    }

    #endregion

    #region ABSTRACT

    public abstract void Iteract(Player source);

    protected abstract float GetLifeTime();


    #endregion

}
