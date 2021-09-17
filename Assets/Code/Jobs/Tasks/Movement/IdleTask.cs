using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class IdleTask : Task
{
    private const float HEAD_RESET = 2.5F;
    private const float R = 25;

    private float timer;
    private float head;
    private float idle;

    private Vector3 targetLook = Vector3.zero;

    public IdleTask(Ninja owner, float idle) : base(owner)
    {
        this.timer = 0;
        this.head = HEAD_RESET;
        this.idle = idle;
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();
        this.timer += Time.deltaTime;
        this.head += Time.deltaTime;

        if (this.timer > idle)
            Succeed();

        if (this.head > HEAD_RESET)
        {
            this.head = 0;
            Vector3 random = new Vector3(Random.Range(-R, R), Random.Range(-1, 2), Random.Range(-R, R));
            targetLook = owner.transform.position + random;
        }
        owner.SetTargetLook(targetLook);
    }

    protected override void DoFixed()
    {
        owner.MoveTorwards(Vector3.zero);
    }
}

