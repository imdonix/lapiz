using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ShurikenItem : Tool
{
    private const string ID = "shuriken";

    public override string GetID()
    {
        return ID;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.ShurikenPref;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().Shuriken;
    }
}

