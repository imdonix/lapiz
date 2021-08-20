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
    [SerializeField] private float Cost;
    [SerializeField] private HandSeal[] Activation;

    protected LivingEntity owner;

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {}

    public abstract void Cast(LivingEntity caster, Vector3 from, Vector3 direction);

    public bool IsSubSubsequent(HandSeal[] seals)
    {
        for (int i = 0; i < Activation.Length; i++)
            if (Activation[i] != seals[i])
                return false;
        return true;    
    }

    public void AttachProperties(LivingEntity owner)
    {
        this.owner = owner;
    }

    public float GetCost()
    {
        return Cost;
    }

}


