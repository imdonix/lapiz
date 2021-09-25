using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class Item : Entity, IInteractable, IEquatable<Item>, IComparable<Item>
{
    private const float DEFAULT_LIFETIME = 30F * 15;

    [Header("Item")]
    private Sprite icon;

    protected Rigidbody rigid;
    protected Collider colider;

    protected float lifeTime = 0;
    protected bool pickedUp = false;

    #region UNITY

    protected override void Awake()
    {
        base.Awake();

        rigid = GetComponent<Rigidbody>();
        colider = GetComponent<Collider>();
    }

    protected override void Update()
    {
        base.Update();

        if (photonView.IsMine)
        {
            if (GetLifeTime() < lifeTime)
                DestroyItem();

            lifeTime += Time.deltaTime;
        }
        else
        {
            rigid.isKinematic = true;
        }
    }

    #endregion

    private void GhostItem() 
    {
        rigid.isKinematic = true;
        colider.enabled = false;
        pickedUp = true;
    }

    private void Materialize()
    {
        rigid.isKinematic = false;
        colider.enabled = true;
        pickedUp = false;
    }

    protected void DestroyItem()
    {
        PhotonNetwork.Destroy(photonView);
    }

    public virtual bool CanInteract()
    {
        return true;
    }

    public virtual string GetDescription()
    {
        return string.Format("{0} {1}", Manager.Instance.GetLanguage().PickUp, GetName());
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public void SetIcon(Sprite sprite)
    {
        this.icon = sprite;
    }

    public bool IsPickUp()
    {
        return pickedUp;
    }

    public void TakeControll()
    {
        photonView.RequestOwnership();
    }

    public virtual void Interact(Ninja source) 
    {
        if (photonView.IsMine)
        {
            GhostItem();
            source.PickUpItem(this);
        }
        else
        {
            if (!pickedUp) 
            {
                GhostItem();
                source.PickUpItem(this);
                TakeControll();
            }
        }
    }

    public void ThrowAway(Vector3 force)
    {
        Materialize();
        rigid.AddForce(force);
    }

    public void PutDown(Vector3 position)
    {
        Materialize();
        transform.position = position;
    }

    public override bool HasTag()
    {
        return false;
    }

    protected virtual float GetLifeTime()
    {
        return DEFAULT_LIFETIME;
    }

    public int CompareTo(Item other)
    {
        return other.GetQuality() - GetQuality();
    }

    public abstract string GetID();

    public abstract Item GetItemPref();

    public abstract ItemQuality GetQuality();

    #region PHOTON

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
        {
            pickedUp = (bool)stream.ReceiveNext();
        }
        else
        {
            stream.SendNext(pickedUp);
        }
    }

    public bool Equals(Item other)
    {
        return GetID() == other.GetID();
    }

    #endregion

}
