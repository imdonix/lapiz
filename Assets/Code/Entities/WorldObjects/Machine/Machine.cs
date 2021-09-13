﻿using System;
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

    #region UNITY

    protected override void Awake()
    {
        base.Awake();
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

        Process();
    }

    protected abstract void ResetInput();

    protected abstract void Store(Item item);

    protected abstract void Process();

    protected Vector3 GetOutputLocation()
    {
        return transform.position + outputLocalPosition;
    }
}