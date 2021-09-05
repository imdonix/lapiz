using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PickAxe : Tool
{
    private const string ID = "pickaxe";

    public override string GetID()
    {
        return ID;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.PickaxePref;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().PickAxe;
    }
}

