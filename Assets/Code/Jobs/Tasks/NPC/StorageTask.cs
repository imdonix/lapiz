using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// The task start with finding the storage carried by the task.
/// </summary>
public abstract class StorageTask : Task
{
    private Storage to;

    public StorageTask(Ninja owner) : base(owner){}

    protected override void DoInit()
    {
        base.DoInit();
        FindStorage();
    }

    public Storage GetStorage()
    {
        return to;
    }

    private void FindStorage()
    {
        Item item = GetCarried();
        if (item == null) throw new Exception("Carried item is null.");

        foreach (Storage storage in Storage.All)
            if (storage.GetItemType().Equals(item.GetItemPref()))
            {
                this.to = storage;
                break;
            }
    }
}
