using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Crafter : Machine, IEquatable<Crafter>
{

    protected List<ICraftable> craftables = new List<ICraftable>();
    protected List<Pair<Item, byte>> storage = new List<Pair<Item, byte>>();
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
            if (recipe.IsMatch(storage))
            {
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
            Pair<Item, byte> found = Pair<Item, byte>.NULL;
            foreach (var pair in storage)
                if (pair.key.Equals(item))
                {
                    found = pair;
                    break;
                }

            if (ReferenceEquals(found, Pair<Item, byte>.NULL))
                storage.Add(new Pair<Item, byte>(item, 1));
            else
                found.value++;
        }
    }

    private void Spawn(ICraftable craftable)
    {
        foreach (Item item in raw) PhotonNetwork.Destroy(item.photonView);
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

