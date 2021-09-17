using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Storage : Machine, IInteractable
{
    public static List<Storage> All = new List<Storage>();

    [Header("Storage | Debug")]
    [SerializeField] private Item stack = null;
    [SerializeField] private int count = 0;

    private List<Item> input = new List<Item>();

    public void InitItem(Item item)
    {
        All.Add(this);
        photonView.RPC("OnItemUpdated", RpcTarget.AllBuffered, item.GetID());
    }

    public bool TakeOne(out Item item)
    {
        if (count > 0)
        {
            photonView.RPC("OnItemRecived", RpcTarget.AllBuffered, count - 1);
            item = PhotonNetwork.Instantiate(
                stack.GetItemPref().name, 
                GetOutputLocation(), 
                Quaternion.identity).GetComponent<Item>();
            return true;
        }
        item = null;
        return false;
    }

    public void Interact(Ninja source)
    {
        TakeOne(out Item _);
    }

    public Item GetItemType()
    {
        return stack.GetItemPref();
    }

    public bool CanInteract()
    {
        return true;
    }

    public override float GetSize()
    {
        return 1.5F;
    }

    public string GetDescription()
    {
        if(count > 0)
            return string.Format("{0} [{1} ({2})]", Manager.Instance.GetLanguage().TakeItem, stack.GetName(), count);
        else
            return string.Format("{0} [{1}]", Manager.Instance.GetLanguage().NoItemAvaiable, stack.GetName());
    }

    protected override void Process()
    {
        if (ReferenceEquals(stack, null)) return;

        foreach (Item item in input)
            if (item.Equals(stack))
            {
                PhotonNetwork.Destroy(item.photonView);
                photonView.RPC("OnItemRecived", RpcTarget.All, count + 1);
            }
    }


    protected override int GetPriority()
    {
        return JobProvider.STORAGE;
    }

    public override Job GetJob(NPC npc)
    {
        throw new NotImplementedException();
    }

    protected override void ResetInput()
    {
        input.Clear();
    }

    protected override void Store(Item item)
    {
        input.Add(item);
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {}

    #region RPC

    [PunRPC]
    public void OnItemUpdated(string itemID)
    {
        this.stack = ItemLibrary.Instance.FindByID(itemID);
        this.count = 0;
    }

    [PunRPC]
    public void OnItemRecived(int count)
    {
        this.count = count;
    }

    #endregion

    public static Storage FindByItem(Item item)
    {
        foreach (Storage storage in Storage.All)
            if (storage.stack.Equals(item))
                return storage;
        Debug.LogWarning(string.Format("Storage not found with {0}", item));
        return null;
    }
}
