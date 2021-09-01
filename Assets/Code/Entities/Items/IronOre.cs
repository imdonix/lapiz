using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class IronOre : Item
{
    public override string GetName()
    {
        return Manager.Instance.GetLanguage().IronOre;
    }

    protected override float GetLifeTime()
    {
        return 60F * 5;
    }
}

