using System.Collections;
using UnityEngine;


public class GoRandomPlaceTask : GoTask
{
    private const int MIN = 30;
    private const int MAX = 80;
    private const float MAX_TIME = 5.5F;

    private float timer = 0;

    public GoRandomPlaceTask(Ninja owner) : base(owner, GetRandomPositonWorldPos()) {}

    protected override void DoUpdate()
    {
        this.timer += Time.deltaTime;
        if (timer > MAX_TIME)
        {
            Succeed();
        }
        else
        {
            base.DoUpdate();
        }

    }

    private static Vector3 GetRandomPositonWorldPos()
    {
        return new Vector3(Random.Range(MIN, MAX), 0, Random.Range(MIN, MAX));
    }
}
