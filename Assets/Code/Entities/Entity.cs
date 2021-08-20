using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviourPun, IPunObservable
{
    public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);
}
