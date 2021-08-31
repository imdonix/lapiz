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

    protected virtual void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            CheckOutOffBound();
        }
    }

    #endregion


    public virtual void Teleport(Vector3 position)
    {
        transform.position = position;
    }

    private void CheckOutOffBound()
    {
        if (transform.position.y < World.BOTTOM)
        {
            Teleport(World.Loaded.GetPlayerSpawnPoint());
        }
    }

    public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);
}
