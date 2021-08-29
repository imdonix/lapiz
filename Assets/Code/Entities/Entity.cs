using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviourPun, IPunObservable
{

    #region UNITY

    protected virtual void Awake() {}

    protected virtual void Start() {}

    protected virtual void Update() {}

    protected virtual void FixedUpdate() {}

    #endregion

    public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);
}
