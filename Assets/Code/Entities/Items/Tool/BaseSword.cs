using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BaseSword : Tool
{
    private const string ID = "sword";

    public override string GetID()
    {
        return ID;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.SwordPref;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().Sword;
    }
}

