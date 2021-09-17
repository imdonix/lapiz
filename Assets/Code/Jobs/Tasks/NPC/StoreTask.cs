using System.Collections;
using UnityEngine;


public class StoreTask : StorageTask
{
    public StoreTask(Ninja owner) : base(owner){}

    protected override void DoInit()
    {
        base.DoInit();
        owner.GetArms().ThrowAway();
        GetStorage().Put(GetCarried());
        Succeed();
    }
}
