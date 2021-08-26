using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Jutsu : Entity
{
    [Header("Jutsu")]
    [SerializeField] private int UnlockLapiz;
    [SerializeField] private float Cost;
    [SerializeField] private HandSeal[] Activation;

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {}

    public void Cast(Ninja caster, Vector3 from, Vector3 direction)
    {
        if(photonView.IsMine) OnCast(caster, from, direction);
        PhotonNetwork.Destroy(gameObject);
    }

    protected abstract void OnCast(Ninja caster, Vector3 from, Vector3 direction);

    public bool IsSubSubsequent(HandSeal[] seals)
    {
        for (int i = 0; i < Activation.Length; i++)
            if (Activation[i] != seals[i])
                return false;
        return true;    
    }

    public float GetCost()
    {
        return Cost;
    }

}


