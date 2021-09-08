using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class HighBlade : Tool
{
    private const string ID = "highblade";

    public override string GetID()
    {
        return ID;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.HighBladePref;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().HighBlade;
    }

    public override ToolType GetToolType()
    {
        return ToolType.Sword;
    }
}

