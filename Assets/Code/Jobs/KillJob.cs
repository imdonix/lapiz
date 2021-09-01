
public class KillJob : Job
{
    private readonly LivingEntity target;

    public KillJob(Ninja owner, LivingEntity target) : base(owner)
    {
        this.target = target;
    }

    protected override void BuildRoutine()
    {
        routine.Add(new FollowTask(owner, target));
    }
}

