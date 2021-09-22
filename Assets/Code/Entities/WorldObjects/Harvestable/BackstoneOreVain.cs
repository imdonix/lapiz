using Photon.Pun;

public class BackstoneOreVain : Harvestable
{
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }

    protected override int GetRate()
    {
        return 30;
    }

    protected override Item GetReward()
    {
        return ItemLibrary.Instance.BackstoneOrePref;
    }

    public override Tool GetCorrectTool()
    {
        return ItemLibrary.Instance.PickaxePref;
    }

    public override float GetSize()
    {
        return 2.35F;
    }

    protected override int GetPriority()
    {
        return base.GetPriority() - 2;
    }

    protected override void OnHarvested() {}
}

