using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Chunin : Invader 
{

    #region UNITY

    protected override void Awake()
    {
        base.Awake();
        head.SetBadge(Village.Kerth);
    }

    protected override void SetupLootTable()
    {
        LootTable.Add(ItemLibrary.Instance.LapizPref);
        LootChance.Add(0.15F);
        LootAmount.Add(1);
    }

    #endregion

}

