using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Crafter : Machine, IEquatable<Crafter>
{
    protected List<ICraftable> craftables = new List<ICraftable>();
    private List<ItemStack> storage = new List<ItemStack>();
    private List<Item> raw = new List<Item>();
    private List<Craft> pending = new List<Craft>();

    #region UNITY

    protected override void Awake()
    {
        base.Awake();
        this.craftables = new List<ICraftable>();
        RegisterCraftables();
    }

    protected override void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            UpdateCrafts();
        }
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
                pending.Add(Craft.Start(craftable, this));
                return;
            }
        }
    }

    protected override void ResetInput()
    {
        raw.Clear();
        storage.Clear();
    }

    public override bool IsWorkAvailable()
    {
        return false;
    }

    public bool IsCrafting() 
    {
        return pending.Count > 0;
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

    private void UpdateCrafts()
    {
        List<Craft> done = new List<Craft>();
        foreach (Craft craft in pending)
        {
            craft.Update();
            if (craft.IsDone())
            {
                craft.End();
                done.Add(craft);
                Spawn(craft.Craftable);
            }
        }

        foreach (Craft craft in done)
            pending.Remove(craft);
    }

    protected abstract void RegisterCraftables();

    public abstract string GetID();

    public abstract string GetName();

    public bool Equals(Crafter other)
    {
        return other.GetID() == GetID();
    }

    protected override int GetPriority()
    {
        return JobProvider.CRAFTER;
    }

    public override Job GetJob(NPC npc)
    {
        throw new NotImplementedException();
    }
}

