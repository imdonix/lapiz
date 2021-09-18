using Photon.Pun;

public class IronOreVain : Harvestable
{
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){}

    protected override int GetRate()
    {
        return 15;
    }

    protected override Item GetReward()
    {
        return ItemLibrary.Instance.IronOrePref;
    }

    public override Tool GetCorrectTool()
    {
        return ItemLibrary.Instance.PickaxePref;
    }

    public override float GetSize()
    {
        return 2.25F;
    }

    protected override void OnHarvested(){}
}


