using Photon.Pun;
using System.Collections;
using UnityEngine;


public class Workstation : Crafter
{
    private const string ID = "workstation";

    public override string GetID()
    {
        return ID;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().Workstation;
    }

    public override float GetSize()
    {
        return 1F;
    }

    #region SERIALIZATION

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }


    #endregion
}
