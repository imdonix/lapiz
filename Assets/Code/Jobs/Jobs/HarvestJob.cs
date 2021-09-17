
public class HarvestJob : Job
{
    private readonly Harvestable harvest;

    public HarvestJob(Ninja owner, Harvestable harvest) : base(owner)
    {
        this.harvest = harvest;
    }

    protected override void BuildRoutine()
    {
        routine.Add(new AcquireTask(owner, harvest.GetCorrectTool()));
        routine.Add(new EquipTask(owner, ToolType.Tool));
        routine.Add(new GoTask(owner, harvest.transform.position, harvest.GetSize()));
        routine.Add(new HarvestTask(owner, harvest));
        routine.Add(new DeliverTask(owner));
        routine.Add(new StoreTask(owner));
    }
}
