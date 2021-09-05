using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class IronOre : Item
{
    private const string ID = "ore_iron";

    public override string GetID()
    {
        return ID;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().IronOre;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.IronOrePref;
    }
}


