using System;
using System.Collections.Generic;
using System.Linq;


public class ItemStack
{
    public readonly Item Prefab;
    public readonly List<Item> Items;

    public int Count => Items.Count;

    public ItemStack(Item prefab, Item item)
    {
        Prefab = prefab;
        Items = new List<Item>() { item };
    }

    public ItemStack(Item prefab)
    {
        Prefab = prefab;
        Items = new List<Item>();
    }

    public void AddItem(Item item)
    {
        Items.Add(item);
    }

    public ItemStack Split(int value)
    {
        if (value > Count) 
            throw new Exception(string.Format("Split into {0} cant be done coz the size of the ItemStack {1}", value, Count));

        ItemStack tmp = new ItemStack(Prefab);
        foreach (Item item in Items.GetRange(0, value))
            tmp.AddItem(item);
        return tmp;
    }
}
