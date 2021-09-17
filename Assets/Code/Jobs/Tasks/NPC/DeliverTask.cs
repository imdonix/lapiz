using System.Collections;
using UnityEngine;


public class DeliverTask : StorageTask
{
    private GoTask go;

    public DeliverTask(Ninja owner) : base(owner){}

    protected override void DoInit()
    {
        base.DoInit();
        this.go = new GoTask(owner, GetStorage().transform.position);
    }

    protected override void DoUpdate()
    {
        if (ReferenceEquals(go, null)) return;
        go.Update();
        if (go.IsDone())
        {
            if (go.IsFailed())
                Fail();
            else
                Succeed(GetCarried());
        }
    }

    protected override void DoFixed()
    {
        if (ReferenceEquals(go, null)) return;
        go.FixedUpdate();
    }
}
