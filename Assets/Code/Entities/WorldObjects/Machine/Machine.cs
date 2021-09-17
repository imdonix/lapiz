using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Machines are Item processing world objects
/// </summary>
public abstract class Machine : WorldObject
{
    [Header("Machine")]
    [SerializeField] private Vector3 inputLocalPosition;
    [SerializeField] private Vector3 inputSize;
    [SerializeField] private Vector3 outputLocalPosition;

    private List<Item> npcRequested;

    #region UNITY

    protected override void Awake()
    {
        base.Awake();
        npcRequested = new List<Item>();
        GetWorld().RegisterMachine(this);
    }

    #endregion

    public void ReadInput()
    {
        ResetInput();
        
        Collider[] colliders = Physics.OverlapBox(transform.position + inputLocalPosition, inputSize / 2, Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            Item item = collider.GetComponent<Item>();
            if (!ReferenceEquals(item, null))
            {
                if (!item.IsPickUp())
                {
                    if(item.photonView.IsMine)
                        Store(item);
                    else
                        item.TakeControll();
                }
            }
        }

        foreach (Item item in npcRequested)
            if (!item.IsPickUp())
                Store(item);
        npcRequested.Clear();


        Process();
    }

    public void Put(Item item)
    {
        npcRequested.Add(item);
    }

    protected abstract void ResetInput();

    protected abstract void Store(Item item);

    protected abstract void Process();

    protected Vector3 GetOutputLocation()
    {
        return transform.position + outputLocalPosition;
    }
}