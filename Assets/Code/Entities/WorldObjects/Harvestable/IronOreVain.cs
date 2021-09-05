using Photon.Pun;

public class IronOreVain : Harvestable
{
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){}

    protected override float GetDropDistacne()
    {
        return 2F;
    }

    protected override int GetRate()
    {
        return 5;
    }

    protected override Item GetReward()
    {
        return ItemLibrary.Instance.IronOrePref;
    }

    protected override bool IsCorrectTool(HandTool tool)
    {
        return tool.GetType().Equals(typeof(HandPickaxe));
    }
}


