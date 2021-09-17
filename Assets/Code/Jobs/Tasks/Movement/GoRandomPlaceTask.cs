using System.Collections;
using UnityEngine;


public class GoRandomPlaceTask : GoTask
{
    private const int RANGE = 30;
    private const float MAX_TIME = 5.5F;

    private float timer = 0;

    public GoRandomPlaceTask(Ninja owner) : base(owner, GetRandomPositon(owner)) {}

    protected override void DoUpdate()
    {
        this.timer += Time.deltaTime;
        if (timer > MAX_TIME)
        {
            Succeed();
        }
        else
        {
            owner.SetTargetLook(target);
            base.DoUpdate();
        }

    }

    private static Vector3 GetRandomPositon(Entity entity)
    {
        return new Vector3(
            entity.transform.position.x + Random.Range(-RANGE, RANGE),
            0,
            entity.transform.position.z + Random.Range(-RANGE, RANGE)
            );
    }
}
