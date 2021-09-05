using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Furnace : Crafter
{

    [Header("Furnace")]
    [SerializeField] private GameObject Flame;

    private const string ID = "furnace";

    private bool isWorking = false;

    #region UNITY

    protected override void Update()
    {
        base.Update();

        if (photonView.IsMine)
        {
            isWorking = IsCrafting();
        }

        Flame.SetActive(isWorking);
    }

    #endregion

    protected override void RegisterCraftables()
    {
        foreach (ICraftable craftable in ItemLibrary.Instance.GetCraftablePrefs(this))
        {
            craftables.Add(craftable);
        }
    }

    public override string GetID()
    {
        return ID;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().Furnace;
    }

    #region SERIALIZATION

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
        {
            isWorking = (bool)stream.ReceiveNext();
        }
        else
        {
            stream.SendNext(isWorking);
        }
    }

    #endregion
}

