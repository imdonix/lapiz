using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class NatureSword : Tool
{
    private const string ID = "naturesword";

    public override string GetID()
    {
        return ID;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.NatureSwordPref;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().NatureSword;
    }

    public override ToolType GetToolType()
    {
        return ToolType.Sword;
    }
}

