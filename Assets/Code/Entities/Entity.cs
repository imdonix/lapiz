using Photon.Pun;
using System;
using UnityEngine;

public abstract class Entity : MonoBehaviourPun, IPunObservable
{

    protected EntityTag myTag;

    #region UNITY

    protected virtual void Awake() 
    {
        this.myTag = InitTag();
    }

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

    public float GetDistance(Entity entity)
    {
        return Vector3.Distance(transform.position, entity.transform.position);
    }

    private void CheckOutOffBound()
    {
        if (transform.position.y < World.BOTTOM)
        {
            Teleport(World.Loaded.GetPlayerSpawnPoint());
        }
    }

    private EntityTag InitTag()
    {
        if (HasTag())
        {
            EntityTag entityTag = Instantiate(HUD.Instance.TagPref, HUD.Instance.Tags.transform).GetComponent<EntityTag>();
            entityTag.Bind(this);
            return entityTag;
        }
        return null;
    }

    public virtual void Update(EntityTag entityTag)
    {
        entityTag.SetName(GetName());
    }

    public virtual string GetName() 
    {
        return string.Empty; 
    }

    public abstract bool HasTag();

    public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);
}
