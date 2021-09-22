using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IHarvestable
{
    public bool Harvest(LivingEntity harvester, int rate, HandTool tool, out Item reward);

    public bool Harvest(LivingEntity harvester, HandTool tool, out Item reward);
}

