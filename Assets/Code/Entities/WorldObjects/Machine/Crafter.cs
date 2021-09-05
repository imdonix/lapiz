﻿using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Crafter : Machine, IEquatable<Crafter>
{

    protected List<ICraftable> craftables = new List<ICraftable>();
    protected List<ItemStack> storage = new List<ItemStack>();
    protected List<Item> raw = new List<Item>();

    #region UNITY

    protected override void Awake()
    {
        base.Awake();
        this.craftables = new List<ICraftable>();
        RegisterCraftables();
    }

    #endregion

    protected override void Store(Item item)
    {
        raw.Add(item);
    }

    protected override void Process()
    {
        AccumulateItems();

        foreach (ICraftable craftable in craftables)
        {
            Recipe recipe = craftable.GetRecipe();
            List<ItemStack> consumable;
            if (recipe.TryCraft(storage, out consumable))
            {
                ConsumeInput(consumable);
                Spawn(craftable);
                return;
            }
        }
    }

    protected override void ResetInput()
    {
        raw.Clear();
        storage.Clear();
    }

    private void AccumulateItems()
    {
        foreach (Item item in raw)
        {
            ItemStack found = null;
            foreach (var pair in storage)
                if (pair.Prefab.Equals(item))
                {
                    found = pair;
                    break;
                }

            if (ReferenceEquals(found, null))
                storage.Add(new ItemStack(item.GetItemPref(), item));
            else
                found.AddItem(item);
        }
    }


    private void ConsumeInput(List<ItemStack> items)
    {
        foreach (ItemStack stack in items)
            foreach (Item item in stack.Items)
                PhotonNetwork.Destroy(item.photonView);
    }

    private void Spawn(ICraftable craftable)
    {
        PhotonNetwork.InstantiateRoomObject(craftable.GetItemPref().name, GetOutputLocation(), Quaternion.identity);
    }

    protected abstract void RegisterCraftables();

    public abstract string GetID();

    public abstract string GetName();

    public bool Equals(Crafter other)
    {
        return other.GetID() == GetID();
    }
}
