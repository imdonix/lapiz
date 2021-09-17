using System.Collections;
using UnityEngine;


public class GoTask : Task
{
    private const float CHECK_PATH = 5F;
    private const float REACH = 0.25F;
    private const float SIMPLE = 5.5F;
    private const float TO_HIGH = 6.5F;

    private Path path = null;
    private PathFindRequest request = null;
    private float pathFindTimer = CHECK_PATH + 1;

    protected readonly Vector3 target;

    public GoTask(Ninja owner, Vector3 target) : base(owner)
    {
        this.target = target;
    }

    protected override void DoUpdate()
    {
        if (!ReferenceEquals(request, null))
        {
            if (request.IsDone())
            {
                if (request.IsSuccessful())
                    path = request.GetPath();
                else
                    path = null;

                request = null;
            }
        }
        else
        {
            if (pathFindTimer > CHECK_PATH)
            {
                pathFindTimer = 0;

                PathFinder finder = World.Loaded.GetPathFinder();

                Vector2Int from = finder.GetGridPosition(owner.transform.position);
                Vector2Int to = finder.GetGridPosition(target);

                request = finder.Request(from, to);
            }
        }

        pathFindTimer += Time.deltaTime;
    }

    protected override void DoFixed()
    {
        base.DoFixed();

        Vector3 highlessMe = new Vector3(owner.transform.position.x, 0, owner.transform.position.z);
        Vector3 highlessTarget = new Vector3(target.x, 0, target.z);
        Vector3 direction = (highlessTarget - highlessMe).normalized;

        float heightDistance = Mathf.Abs(owner.transform.position.y - target.y);
        float airDistance = Vector3.Distance(highlessTarget, highlessMe);

        if (ReferenceEquals(path, null) || airDistance < SIMPLE || heightDistance > TO_HIGH)
        {
            if (airDistance > REACH * 6F)
                owner.MoveTorwards(direction);
            else
                Succeed();
        }
        else
        {
            Vector3 checkPoint = path.Current().GetRealPos();

            direction = (checkPoint - highlessMe).normalized;
            if (Vector3.Distance(checkPoint, highlessMe) < REACH)
                if (path.HasNext())
                {
                    path.Next();
                }
                else
                {
                    path = null;
                    direction = Vector3.zero;
                    pathFindTimer = CHECK_PATH + 1;
                }

            owner.MoveTorwards(direction);
        }
    }
}
