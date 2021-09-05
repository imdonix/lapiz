using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Axe : Tool
{
    private const string ID = "axe";

    public override string GetID()
    {
        return ID;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.AxePref;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().Axe;
    }
}

