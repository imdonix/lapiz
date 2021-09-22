

public class BackstoneOre : Item
{

    private const string ID = "ore_backstone";

    public override string GetID()
    {
        return ID;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().BackstoneOre;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.BackstoneOrePref;
    }

    public override ItemQuality GetQuality()
    {
        return ItemQuality.COMMON;
    }
}

